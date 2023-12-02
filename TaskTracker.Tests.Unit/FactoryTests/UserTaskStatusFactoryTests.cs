using TaskTracker.Application;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class UserTaskStatusFactoryTests
    {
        private readonly UserTaskStatusFactory _factory;

        public UserTaskStatusFactoryTests()
        {
            _factory = new UserTaskStatusFactory();
        }

        [Fact]
        public void Create_WithIsDefault_CreatesValidEntity()
        {
            var request = new AddUserTaskStatusRequest
            {
                Color = "c",
                GroupId = 1,
                Index = 2,
                Name = "name",
            };

            const bool IsDefault = true;

            var status = _factory.Create(request, IsDefault);

            Assert.Equal(request.Name, status.Name);
            Assert.Equal(request.Color, status.Color);
            Assert.Equal(request.Index, status.Index);
            Assert.Equal(request.GroupId, status.GroupId);
            Assert.Equal(IsDefault, status.IsDefault);
        }

        [Fact]
        public void Create_WithoutIsDefault_CreatesValidEntity()
        {
            var request = new AddUserTaskStatusRequest
            {
                Color = "c",
                GroupId = 1,
                Index = 2,
                Name = "name",
            };

            var status = _factory.Create(request);

            Assert.Equal(request.Name, status.Name);
            Assert.Equal(request.Color, status.Color);
            Assert.Equal(request.Index, status.Index);
            Assert.Equal(request.GroupId, status.GroupId);
            Assert.False(status.IsDefault);
        }
    }
}
