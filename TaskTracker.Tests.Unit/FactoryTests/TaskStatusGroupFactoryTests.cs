using TaskTracker.Application;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class TaskStatusGroupFactoryTests 
    {
        private readonly TaskStatusGroupFactory _factory;

        public TaskStatusGroupFactoryTests()
        {
            _factory = new TaskStatusGroupFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddTaskStatusGroupRequest
            {
                Name = "name",
                UserId = 2
            };

            const bool IsDefault = true;

            var result = _factory.Create(request, IsDefault);

            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.UserId, result.UserId);
            Assert.Equal(IsDefault, result.IsDefault);
        }
    }
}
