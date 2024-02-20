using TaskTracker.Application;
using TaskTracker.Model.SpaceUserPermissions.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class SpaceUserPermissionsFactoryTests
    {
        private readonly SpaceUserPermissionsFactory _factory;

        public SpaceUserPermissionsFactoryTests()
        {
            _factory = new SpaceUserPermissionsFactory();
        }

        [Fact]
        public void Create_AllDefaultPermissions_CreatesValidEntity()
        {
            var request = new AddSpaceUserPermissionsRequest
            {
                UserId = 1,
                SpaceId = 2,
            };

            var permissions = _factory.Create(request);

            Assert.Equal(request.UserId, permissions.UserId);
            Assert.Equal(request.SpaceId, permissions.SpaceId);
            Assert.False(permissions.CanRemoveLists);
            Assert.False(permissions.CanRemoveUsers);
            Assert.False(permissions.CanAddUsers);
            Assert.False(permissions.CanModifyLists);
            Assert.False(permissions.CanAssignTaskUsers);
            Assert.False(permissions.CanRemoveTasks);
            Assert.False(permissions.CanChangePermissions);
            Assert.False(permissions.CanModifySpace);
            Assert.False(permissions.CanRemoveTasks);
            Assert.False(permissions.CanModifyTasks);
        }

        [Fact]
        public void Create_AllPermissionsSet_CreatesValidEntity()
        {
            var request = new AddSpaceUserPermissionsRequest
            {
                UserId = 1,
                SpaceId = 2,
                CanAddUsers = true,
                CanChangePermissions = true,
                CanAssignTaskUsers = true,
                CanModifyLists = true,
                CanModifySpace = true,
                CanModifyStatusGroups = true,
                CanModifyTasks = true,
                CanRemoveLists = true,
                CanRemoveTasks = true,
                CanRemoveUsers = true,
            };

            var permissions = _factory.Create(request);

            Assert.Equal(request.UserId, permissions.UserId);
            Assert.Equal(request.SpaceId, permissions.SpaceId);
            Assert.True(permissions.CanRemoveLists);
            Assert.True(permissions.CanRemoveUsers);
            Assert.True(permissions.CanAddUsers);
            Assert.True(permissions.CanModifyLists);
            Assert.True(permissions.CanAssignTaskUsers);
            Assert.True(permissions.CanRemoveTasks);
            Assert.True(permissions.CanChangePermissions);
            Assert.True(permissions.CanModifySpace);
            Assert.True(permissions.CanRemoveTasks);
            Assert.True(permissions.CanModifyTasks);
        }
    }
}
