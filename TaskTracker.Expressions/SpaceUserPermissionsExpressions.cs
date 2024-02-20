using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUserPermissions;

namespace TaskTracker.Expressions
{
    public static class SpaceUserPermissionsExpressions
    {
        public static Expression<Func<SpaceUserPermissions, SpaceUserPermissionsModel>> Model { get; } = permissions => new SpaceUserPermissionsModel
        {
            User = UserExpressions.Model.Compile().Invoke(permissions.User),
            SpaceId = permissions.SpaceId,
            CanAddUsers = permissions.CanAddUsers,
            CanAssignTaskUsers = permissions.CanAssignTaskUsers,
            CanChangePermissions = permissions.CanChangePermissions,
            CanModifyLists = permissions.CanModifyLists,
            CanModifySpace = permissions.CanModifySpace,
            CanModifyStatusGroups = permissions.CanModifyStatusGroups,
            CanModifyTasks = permissions.CanModifyTasks,
            CanRemoveLists = permissions.CanRemoveLists,
            CanRemoveTasks = permissions.CanRemoveTasks,
            CanRemoveUsers = permissions.CanRemoveUsers,
        };
    }
}
