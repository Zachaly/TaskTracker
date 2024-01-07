using TaskTracker.Application;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class SpaceUserFactoryTests
    {
        private readonly SpaceUserFactory _factory;

        public SpaceUserFactoryTests()
        {
            _factory = new SpaceUserFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddSpaceUserRequest
            {
                SpaceId = 1,
                UserId = 2
            };

            var user = _factory.Create(request);

            Assert.Equal(request.SpaceId, user.SpaceId);
            Assert.Equal(request.UserId, user.UserId);
        }
    }
}
