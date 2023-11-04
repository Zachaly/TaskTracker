using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateTaskListCommand : UpdateTaskListRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateTaskListHandler : IRequestHandler<UpdateTaskListCommand, ResponseModel>
    {
        private readonly ITaskListRepository _taskListRepository;
        private readonly IValidator<UpdateTaskListCommand> _validator;

        public UpdateTaskListHandler(ITaskListRepository taskListRepository, IValidator<UpdateTaskListCommand> validator)
        {
            _taskListRepository = taskListRepository;
            _validator = validator;
        }

        public Task<ResponseModel> Handle(UpdateTaskListCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
