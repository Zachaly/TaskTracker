using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateUserTaskCommand : UpdateUserTaskRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateUserTaskHandler : IRequestHandler<UpdateUserTaskCommand, ResponseModel>
    {
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IValidator<UpdateUserTaskCommand> _validator;

        public UpdateUserTaskHandler(IUserTaskRepository userTaskRepository, IValidator<UpdateUserTaskCommand> validator)
        {
            _userTaskRepository = userTaskRepository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateUserTaskCommand request, CancellationToken cancellationToken)
        {
            var validaton = await _validator.ValidateAsync(request);

            if (!validaton.IsValid)
            {
                return new ResponseModel(validaton.ToDictionary());
            }

            var task = await _userTaskRepository.GetByIdAsync(request.Id, t => t);

            if(task is null)
            {
                return new ResponseModel("Entity not found");
            }

            task.Title = request.Title ?? task.Title;
            task.Description = request.Description ?? task.Description;
            task.DueTimestamp = request.DueTimestamp;
            task.StatusId = request.StatusId ?? task.StatusId;

            await _userTaskRepository.UpdateAsync(task);

            return new ResponseModel();
        }
    }
}
