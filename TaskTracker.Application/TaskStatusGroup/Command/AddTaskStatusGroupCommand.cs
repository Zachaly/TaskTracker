using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskStatusGroup.Request;

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

        public Task<CreatedResponseModel> Handle(AddTaskStatusGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
