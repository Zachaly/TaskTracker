using TaskTracker.Application.Abstraction;
using TaskTracker.Database;
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
        protected GetUserSpaceByIdHandler(IUserSpaceRepository repository) : base(repository)
        {
        }
    }
}
