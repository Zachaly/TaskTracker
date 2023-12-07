using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateUserTaskStatusCommand : UpdateUserTaskStatusRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateUserTaskStatusHandler : IRequestHandler<UpdateUserTaskStatusCommand, ResponseModel>
    {
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;
        private readonly IValidator<UpdateUserTaskStatusCommand> _validator;

        public UpdateUserTaskStatusHandler(IUserTaskStatusRepository userTaskStatusRepository,
            IValidator<UpdateUserTaskStatusCommand> validator)
        {
            _userTaskStatusRepository = userTaskStatusRepository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateUserTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var status = await _userTaskStatusRepository.GetByIdAsync(request.Id, x => x);

            if(status is null)
            {
                return new ResponseModel("Entity not found");
            }

            if(status.IsDefault && request.Index.GetValueOrDefault() != 0 && request.Index.GetValueOrDefault() != 21)
            {
                return new ResponseModel("Cannot change index of default status!");
            }

            var updateOtherStatuses = status.Index != request.Index && request.Index is not null;

            int originalIndex = status.Index;

            status.Name = request.Name ?? status.Name;
            status.Index = request.Index ?? status.Index;
            status.Color = request.Color ?? status.Color;

            await _userTaskStatusRepository.UpdateAsync(status);

            if (updateOtherStatuses)
            {
                await UpdateGroupStatuses(status, originalIndex);
            }

            return new ResponseModel();
        }

        private async Task UpdateGroupStatuses(UserTaskStatus updatedStatus, int originalIndex)
        {
            var statuses = await _userTaskStatusRepository.GetAsync(new GetUserTaskStatusRequest
            {
                GroupId = updatedStatus.GroupId,
                IsDefault = false,
                SkipPagination = true
            }, x => x);

            var nextStatus = statuses.FirstOrDefault(s => s.Id != updatedStatus.Id && s.Index == updatedStatus.Index);

            if(nextStatus is null)
            {
                return;
            }

            if(originalIndex > updatedStatus.Index)
            {
                nextStatus.Index++;
            }
            else
            {
                nextStatus.Index--;
            }
            await _userTaskStatusRepository.UpdateAsync(nextStatus);
        }
    }
}
