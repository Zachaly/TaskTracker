using MediatR;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public record DeleteTaskAssignedUserCommand(long TaskId, long UserId) : IRequest<ResponseModel>;

    public class DeleteTaskAssignedUserHandler : IRequestHandler<DeleteTaskAssignedUserCommand, ResponseModel>
    {
        private readonly ITaskAssignedUserRepository _repository;

        public DeleteTaskAssignedUserHandler(ITaskAssignedUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseModel> Handle(DeleteTaskAssignedUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteByTaskIdAndUserIdAsync(request.TaskId, request.UserId);

                return new ResponseModel();
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }
        }
    }
}
