using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUserPermissions.Request;

namespace TaskTracker.Application
{
    public interface ISpaceUserPermissionsFactory : IEntityFactory<SpaceUserPermissions, AddSpaceUserPermissionsRequest>
    {

    }

    public class SpaceUserPermissionsFactory : ISpaceUserPermissionsFactory
    {
        public SpaceUserPermissions Create(AddSpaceUserPermissionsRequest request)
            => new SpaceUserPermissions
            {
                SpaceId = request.SpaceId,
                UserId = request.UserId,
                CanAddUsers = request.CanAddUsers.GetValueOrDefault(),
                CanChangePermissions = request.CanChangePermissions.GetValueOrDefault(),
                CanAssignTaskUsers = request.CanAssignTaskUsers.GetValueOrDefault(),
                CanModifyLists = request.CanModifyLists.GetValueOrDefault(),
                CanModifySpace = request.CanModifySpace.GetValueOrDefault(),
                CanModifyStatusGroups = request.CanModifyStatusGroups.GetValueOrDefault(),
                CanModifyTasks = request.CanModifyTasks.GetValueOrDefault(),
                CanRemoveLists = request.CanRemoveLists.GetValueOrDefault(),
                CanRemoveTasks = request.CanRemoveTasks.GetValueOrDefault(),
                CanRemoveUsers = request.CanRemoveUsers.GetValueOrDefault(),
            };
    }
}
