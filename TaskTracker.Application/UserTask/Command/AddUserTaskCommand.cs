using FluentValidation;
using MediatR;
using TaskTracker.Application.Validator;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class AddUserTaskCommand : AddUserTaskRequest, IRequest<CreatedResponseModel>
    {
    }

    public class AddUserTaskHandler : IRequestHandler<AddUserTaskCommand, ResponseModel>
    {
        private readonly IUserTaskFactory _userTaskFactory;
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IValidator<AddUserTaskCommand> _validator;

        public AddUserTaskHandler(IUserTaskFactory userTaskFactory, IUserTaskRepository userTaskRepository, 
            IValidator<AddUserTaskCommand> validator)
        {
            _userTaskFactory = userTaskFactory;
            _userTaskRepository = userTaskRepository;
            _validator = validator;
        }

        public Task<ResponseModel> Handle(AddUserTaskCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
