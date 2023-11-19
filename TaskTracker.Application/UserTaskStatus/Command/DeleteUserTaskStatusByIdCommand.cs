using MediatR;
using TaskTracker.Database.Exception;
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

        public async Task<ResponseModel> Handle(DeleteUserTaskStatusByIdCommand request, CancellationToken cancellationToken)
        {
            var isDefault = await _userTaskStatusRepository.GetByIdAsync(request.Id, x => x.IsDefault);

            if(isDefault)
            {
                return new ResponseModel("This status cannot be deleted");
            }

            try 
            {
                await _userTaskStatusRepository.DeleteByIdAsync(request.Id);
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }

            return new ResponseModel();
        }
    }
}
