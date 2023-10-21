using TaskTracker.Application;

namespace TaskTracker.Tests.Unit.Factory
{
    public class RefreshTokenFactoryTests
    {
        private readonly RefreshTokenFactory _factory;

        public RefreshTokenFactoryTests()
        {
            _factory = new RefreshTokenFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            const long UserId = 1;
            const string Token = "token";

            var refreshToken = _factory.Create(UserId, Token);

            Assert.Equal(Token, refreshToken.Token);
            Assert.Equal(UserId, refreshToken.UserId);
            Assert.NotEqual(default, refreshToken.CreationDate);
            Assert.NotEqual(default, refreshToken.ExpiryDate);
            Assert.True(refreshToken.ExpiryDate > refreshToken.CreationDate);
            Assert.True(refreshToken.IsValid);
        }
    }
}
