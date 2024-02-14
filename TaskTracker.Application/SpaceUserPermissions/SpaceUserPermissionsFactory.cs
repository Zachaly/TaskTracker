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
                CanAddUsers = request.CanAddUsers,
                CanChangePermissions = request.CanChangePermissions,
                CanAssignTaskUsers = request.CanAssignTaskUsers,
                CanModifyLists = request.CanModifyLists,
                CanModifySpace = request.CanModifySpace,
                CanModifyStatusGroups = request.CanModifyStatusGroups,
                CanModifyTasks = request.CanModifyTasks,
                CanRemoveLists = request.CanRemoveLists,
                CanRemoveTasks = request.CanRemoveTasks,
                CanRemoveUsers = request.CanRemoveUsers,
            };
    }
}
