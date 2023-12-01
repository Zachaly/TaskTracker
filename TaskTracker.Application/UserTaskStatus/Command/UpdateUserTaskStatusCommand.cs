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

            var updateOtherStatuses = status.Index != request.Index && request.Index is not null;

            status.Name = request.Name ?? status.Name;
            status.Index = request.Index ?? status.Index;
            status.Color = request.Color ?? status.Color;

            await _userTaskStatusRepository.UpdateAsync(status);

            if (updateOtherStatuses)
            {
                await UpdateGroupStatuses(status);
            }

            return new ResponseModel();
        }

        private async Task UpdateGroupStatuses(UserTaskStatus updatedStatus)
        {
            var statuses = await _userTaskStatusRepository.GetAsync(new GetUserTaskStatusRequest
            {
                GroupId = updatedStatus.GroupId,
                IsDefault = false,
                SkipPagination = true
            }, x => x);

            foreach (var status in statuses.ToList().Where(x => x.Index >= updatedStatus.Index && updatedStatus.Id != x.Id))
            {
                status.Index--;
                await _userTaskStatusRepository.UpdateAsync(status);
            }
        }
    }
}
