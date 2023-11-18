using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
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
        public Task<ResponseModel> Handle(UpdateUserTaskStatusCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
