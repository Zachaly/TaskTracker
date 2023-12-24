using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserSpaceQuery : GetUserSpaceRequest, IGetEntityQuery<UserSpaceModel>
    {
    }

    public class GetUserSpaceHandler : GetEntityHandler<UserSpace, UserSpaceModel, GetUserSpaceRequest, GetUserSpaceQuery>
    {
        public GetUserSpaceHandler(IUserSpaceRepository repository) : base(repository)
        {
        }
    }
}
