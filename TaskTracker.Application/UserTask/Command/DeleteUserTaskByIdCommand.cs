using MediatR;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public class DeleteUserTaskByIdCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteUserTaskByIdHandler : IRequestHandler<DeleteUserTaskByIdCommand, ResponseModel>
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public DeleteUserTaskByIdHandler(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository;
        }

        public async Task<ResponseModel> Handle(DeleteUserTaskByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userTaskRepository.DeleteByIdAsync(request.Id);

                return new ResponseModel();
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }
        }
    }
}
