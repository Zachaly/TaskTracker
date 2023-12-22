using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;

namespace TaskTracker.Application.Command
{
    public class GetUserSpaceByIdQuery : GetEntityByIdQuery<UserSpaceModel>
    {
    }

    public class GetUserSpaceByIdHandler : GetEntityByIdHandler<UserSpace, UserSpaceModel, GetUserSpaceByIdQuery>
    {
        public GetUserSpaceByIdHandler(IUserSpaceRepository repository) : base(repository)
        {
        }
    }
}
