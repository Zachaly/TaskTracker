using MediatR;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public class DeleteSpaceUserCommand : IRequest<ResponseModel>
    {
        public long SpaceId { get; set; }
        public long UserId { get; set; }

        public DeleteSpaceUserCommand(long spaceId, long userId)
        {
            SpaceId = spaceId;
            UserId = userId;
        }
    }

    public class DeleteSpaceUserHandler : IRequestHandler<DeleteSpaceUserCommand, ResponseModel>
    {
        private readonly ISpaceUserRepository _spaceUserRepository;

        public DeleteSpaceUserHandler(ISpaceUserRepository spaceUserRepository)
        {
            _spaceUserRepository = spaceUserRepository;
        }

        public async Task<ResponseModel> Handle(DeleteSpaceUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _spaceUserRepository.DeleteByUserIdAndSpaceIdAsync(request.UserId, request.SpaceId);

                return new ResponseModel();
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }
        }
    }
}
