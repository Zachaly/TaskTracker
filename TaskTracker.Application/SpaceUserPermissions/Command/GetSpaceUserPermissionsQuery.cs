using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUserPermissions;
using TaskTracker.Model.SpaceUserPermissions.Request;

namespace TaskTracker.Application.Command
{
    public class GetSpaceUserPermissionsQuery : GetSpaceUserPermissionsRequest, IGetEntityQuery<SpaceUserPermissionsModel>
    {
    }

    public class GetSpaceUserPermissionsHandler : GetEntityHandler<SpaceUserPermissions, SpaceUserPermissionsModel,
        GetSpaceUserPermissionsRequest, GetSpaceUserPermissionsQuery>
    {
        public GetSpaceUserPermissionsHandler(ISpaceUserPermissionsRepository repository) : base(repository)
        {
        }
    }
}
