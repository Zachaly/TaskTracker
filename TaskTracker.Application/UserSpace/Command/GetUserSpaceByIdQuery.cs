using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserSpaceByIdQuery : GetEntityByIdQuery<UserSpaceModel>
    {
    }

    public class GetUserSpaceByIdHandler : GetEntityByIdHandler<UserSpace, UserSpaceModel, GetUserSpaceRequest, GetUserSpaceByIdQuery>
    {
        public GetUserSpaceByIdHandler(IUserSpaceRepository repository) : base(repository)
        {
        }
    }
}
