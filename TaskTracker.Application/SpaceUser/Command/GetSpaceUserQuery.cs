using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUser;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Application.Command
{
    public class GetSpaceUserQuery : GetSpaceUserRequest, IGetEntityQuery<SpaceUserModel>
    {
    }

    public class GetSpaceUserHandler : GetEntityHandler<SpaceUser, SpaceUserModel, GetSpaceUserRequest, GetSpaceUserQuery>
    {
        public GetSpaceUserHandler(ISpaceUserRepository repository) : base(repository) { }
    }
}
