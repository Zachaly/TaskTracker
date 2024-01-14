using TaskTracker.Application;
using TaskTracker.Model.TaskAssignedUser.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class TaskAssignedUserFactoryTests
    {
        private readonly TaskAssignedUserFactory _factory;

        public TaskAssignedUserFactoryTests()
        {
            _factory = new TaskAssignedUserFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddTaskAssignedUserRequest
            {
                TaskId = 1,
                UserId = 2,
            };

            var user = _factory.Create(request);

            Assert.Equal(request.TaskId, user.TaskId);
            Assert.Equal(request.UserId, user.UserId);
        }
    }
}
