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

    public class AddUserTaskHandler : IRequestHandler<AddUserTaskCommand, CreatedResponseModel>
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

        public async Task<CreatedResponseModel> Handle(AddUserTaskCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);

            if(!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var task = _userTaskFactory.Create(request);

            var id = await _userTaskRepository.AddAsync(task);

            return new CreatedResponseModel(id);
        }
    }
}
