using TaskTracker.Application;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Tests.Unit.Factory
{
    public class UserFactoryTests
    {
        private readonly UserFactory _factory;

        public UserFactoryTests()
        {
            _factory = new UserFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new RegisterRequest
            {
                Email = "email",
                FirstName = "fname",
                LastName = "lname",
            };

            const string PasswordHash = "hash";

            var user = _factory.Create(request, PasswordHash);

            Assert.Equal(PasswordHash, user.PasswordHash);
            Assert.Equal(request.Email, user.Email);
            Assert.Equal(request.LastName, user.LastName);
            Assert.Equal(request.FirstName, user.FirstName);
        }
    }
}
