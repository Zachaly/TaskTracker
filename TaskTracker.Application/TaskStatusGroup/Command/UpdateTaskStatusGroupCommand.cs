using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateTaskStatusGroupCommand : UpdateTaskStatusGroupRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateTaskStatusGroupHandler : IRequestHandler<UpdateTaskStatusGroupCommand, ResponseModel>
    {
        private readonly ITaskStatusGroupRepository _taskStatusGroupRepository;
        private readonly IValidator<UpdateTaskStatusGroupCommand> _validator;

        public UpdateTaskStatusGroupHandler(ITaskStatusGroupRepository taskStatusGroupRepository,
            IValidator<UpdateTaskStatusGroupCommand> validator)
        {
            _taskStatusGroupRepository = taskStatusGroupRepository;
            _validator = validator;
        }

        public Task<ResponseModel> Handle(UpdateTaskStatusGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
