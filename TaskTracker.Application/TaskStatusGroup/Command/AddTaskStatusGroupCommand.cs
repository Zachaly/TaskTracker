using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskStatusGroup.Request;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class AddTaskStatusGroupCommand : AddTaskStatusGroupRequest, IRequest<CreatedResponseModel>
    {
    }

    public class AddTaskStatusGroupHandler : IRequestHandler<AddTaskStatusGroupCommand, CreatedResponseModel>
    {
        private readonly ITaskStatusGroupRepository _taskStatusGroupRepository;
        private readonly ITaskStatusGroupFactory _taskStatusGroupFactory;
        private readonly IValidator<AddTaskStatusGroupCommand> _validator;
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;
        private readonly IUserTaskStatusFactory _userTaskStatusFactory;

        public AddTaskStatusGroupHandler(ITaskStatusGroupRepository taskStatusGroupRepository, ITaskStatusGroupFactory taskStatusGroupFactory,
            IValidator<AddTaskStatusGroupCommand> validator, IUserTaskStatusRepository userTaskStatusRepository,
            IUserTaskStatusFactory userTaskStatusFactory)
        {
            _taskStatusGroupRepository = taskStatusGroupRepository;
            _taskStatusGroupFactory = taskStatusGroupFactory;
            _validator = validator;
            _userTaskStatusRepository = userTaskStatusRepository;
            _userTaskStatusFactory = userTaskStatusFactory;
        }

        public async Task<CreatedResponseModel> Handle(AddTaskStatusGroupCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var group = _taskStatusGroupFactory.Create(request);

            var statusGroupId = await _taskStatusGroupRepository.AddAsync(group);

            var backlogStatus = _userTaskStatusFactory.Create(new AddUserTaskStatusRequest
            {
                Color = "#4d4e4f",
                GroupId = statusGroupId,
                Index = 0,
                Name = "Backlog"
            }, true);

            var closedStatus = _userTaskStatusFactory.Create(new AddUserTaskStatusRequest
            {
                Color = "#08ad05",
                GroupId = statusGroupId,
                Index = 21,
                Name = "Closed"
            }, true);

            await _userTaskStatusRepository.AddAsync(backlogStatus, closedStatus);

            return new CreatedResponseModel(statusGroupId);
        }


    }
}
