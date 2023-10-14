using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Application.Authorization.Command;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Unit.Command
{
    public class AuthorizationCommandTests
    {
        [Fact]
        public async Task LoginCommand_Success()
        {
            var user = new User
            {
                Id = 1,
                Email = "email",
                FirstName = "fname",
                LastName = "lname"
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).Returns(user);

            var hashService = Substitute.For<IHashService>();

            hashService.CompareStringWithHashAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var tokenService = Substitute.For<ITokenService>();

            const string AccessToken = "acctoken";
            const string RefreshToken = "refresh";

            tokenService.GenerateAccessTokenAsync(Arg.Any<User>()).Returns(AccessToken);
            tokenService.GenerateRefreshTokenAsync().Returns(RefreshToken);

            var command = new LoginCommand
            {
                Email = "email",
                Password = "password"
            };

            var handler = new LoginCommandHandler(userRepository, hashService, tokenService);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(RefreshToken, result.Data.RefreshToken);
            Assert.Equal(AccessToken, result.Data.AccessToken);
            Assert.Equal(user.FirstName, result.Data.UserData.FirstName);
            Assert.Equal(user.LastName, result.Data.UserData.LastName);
            Assert.Equal(user.Id, result.Data.UserData.Id);
            Assert.Equal(user.Email, result.Data.UserData.Email);
            Assert.Equal(RefreshToken, user.RefreshToken);
        }

        [Fact]
        public async Task LoginCommand_InvalidEmail_Failure()
        {

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).ReturnsNull();

            var hashService = Substitute.For<IHashService>();

            var tokenService = Substitute.For<ITokenService>();

            var command = new LoginCommand
            {
                Email = "email",
                Password = "password"
            };

            var handler = new LoginCommandHandler(userRepository, hashService, tokenService);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Error);
        }

        [Fact]
        public async Task LoginCommand_InvalidPassword_Failure()
        {
            var user = new User
            {
                Id = 1,
                Email = "email",
                FirstName = "fname",
                LastName = "lname"
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).Returns(user);

            var hashService = Substitute.For<IHashService>();

            hashService.CompareStringWithHashAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            var tokenService = Substitute.For<ITokenService>();

            var command = new LoginCommand
            {
                Email = "email",
                Password = "password"
            };

            var handler = new LoginCommandHandler(userRepository, hashService, tokenService);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Error);
        }
    }
}
