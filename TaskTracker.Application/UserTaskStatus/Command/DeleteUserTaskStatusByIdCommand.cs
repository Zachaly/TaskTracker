using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public class DeleteUserTaskStatusByIdCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteUserTaskStatusByIdHandler : IRequestHandler<DeleteUserTaskStatusByIdCommand, ResponseModel>
    {
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;

        public DeleteUserTaskStatusByIdHandler(IUserTaskStatusRepository userTaskStatusRepository)
        {
            _userTaskStatusRepository = userTaskStatusRepository;
        }

        public Task<ResponseModel> Handle(DeleteUserTaskStatusByIdCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
