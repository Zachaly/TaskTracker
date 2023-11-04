using TaskTracker.Application;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class UserTaskFactoryTests
    {
        private readonly UserTaskFactory _factory;

        public UserTaskFactoryTests()
        {
            _factory = new UserTaskFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddUserTaskRequest
            {
                CreatorId = 1,
                Description = "desc",
                DueTimestamp = 2,
                Title = "title",
                ListId = 3
            };

            var task = _factory.Create(request);

            Assert.Equal(request.CreatorId, task.CreatorId);
            Assert.Equal(request.Description, task.Description);
            Assert.Equal(request.Title, task.Title);    
            Assert.Equal(request.DueTimestamp, task.DueTimestamp);
            Assert.Equal(request.ListId, task.ListId);
            Assert.NotEqual(default, task.CreationTimestamp);
        }
    }
}
