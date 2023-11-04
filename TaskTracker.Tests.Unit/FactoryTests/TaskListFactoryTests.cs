using TaskTracker.Application;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class TaskListFactoryTests
    {
        private readonly TaskListFactory _factory;

        public TaskListFactoryTests()
        {
            _factory = new TaskListFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddTaskListRequest
            {
                Color = "#000000",
                CreatorId = 1,
                Description = "desc",
                Title = "title",
            };

            var list = _factory.Create(request);

            Assert.Equal(request.Title, list.Title);
            Assert.Equal(request.CreatorId, list.CreatorId);
            Assert.Equal(request.Color, list.Color);
            Assert.Equal(request.Description, list.Description);
        }
    }
}
