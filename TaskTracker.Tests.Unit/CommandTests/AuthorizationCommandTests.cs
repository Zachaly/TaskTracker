using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Transactions;
using TaskTracker.Application;
using TaskTracker.Application.Authorization.Command;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Tests.Unit.ValidatorTests;

namespace TaskTracker.Tests.Unit.CommandTests
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

            var tokens = new List<RefreshToken>();

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).Returns(user);

            var hashService = Substitute.For<IHashService>();

            hashService.CompareStringWithHashAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var tokenService = Substitute.For<ITokenService>();

            const string AccessToken = "acctoken";
            const string RefreshToken = "refresh";

            tokenService.GenerateAccessTokenAsync(Arg.Any<User>()).Returns(AccessToken);
            tokenService.GenerateRefreshTokenAsync().Returns(RefreshToken);

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            tokenRepository.GetTokenAsync(RefreshToken).ReturnsNull();

            tokenRepository.AddAsync(Arg.Any<RefreshToken>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => tokens.Add(info.Arg<RefreshToken>()));

            var tokenFactory = Substitute.For<IRefreshTokenFactory>();

            tokenFactory.Create(Arg.Any<long>(), Arg.Any<string>()).Returns(info => new RefreshToken
            {
                UserId = info.Arg<long>(),
                Token = info.Arg<string>()
            });

            var command = new LoginCommand
            {
                Email = "email",
                Password = "password"
            };

            var handler = new LoginCommandHandler(userRepository, hashService, tokenService, tokenRepository, tokenFactory);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(RefreshToken, result.Data.RefreshToken);
            Assert.Equal(AccessToken, result.Data.AccessToken);
            Assert.Equal(user.FirstName, result.Data.UserData.FirstName);
            Assert.Equal(user.LastName, result.Data.UserData.LastName);
            Assert.Equal(user.Id, result.Data.UserData.Id);
            Assert.Equal(user.Email, result.Data.UserData.Email);
            Assert.Contains(tokens, t => t.UserId == user.Id && t.Token == RefreshToken);
        }

        [Fact]
        public async Task LoginCommand_InvalidEmail_Failure()
        {

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).ReturnsNull();

            var hashService = Substitute.For<IHashService>();

            var tokenService = Substitute.For<ITokenService>();

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            var tokenFactory = Substitute.For<IRefreshTokenFactory>();

            var command = new LoginCommand
            {
                Email = "email",
                Password = "password"
            };

            var handler = new LoginCommandHandler(userRepository, hashService, tokenService, tokenRepository, tokenFactory);

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

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            var tokenFactory = Substitute.For<IRefreshTokenFactory>();

            var command = new LoginCommand
            {
                Email = "email",
                Password = "password"
            };

            var handler = new LoginCommandHandler(userRepository, hashService, tokenService, tokenRepository, tokenFactory);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Error);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_Success()
        {
            var user = new User
            {
                PasswordHash = "hash"
            };

            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                CurrentPassword = "password",
                NewPassword = "new password"
            };

            var tokens = new List<RefreshToken>()
            {
                new RefreshToken { IsValid = true },
                new RefreshToken { IsValid = true },
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByIdAsync(command.UserId, Arg.Any<Func<User, User>>()).Returns(user);
            userRepository.UpdateAsync(user).Returns(Task.CompletedTask);

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();
            tokenRepository.GetByUserIdAsync(user.Id).Returns(tokens);

            tokenRepository.UpdateAsync(Arg.Any<RefreshToken>()).Returns(Task.CompletedTask);

            const string NewHash = "new hash";
            var hashService = Substitute.For<IHashService>();
            hashService.HashStringAsync(command.NewPassword).Returns(NewHash);

            hashService.CompareStringWithHashAsync(command.CurrentPassword, user.PasswordHash).Returns(true);

            var validator = Substitute.For<IValidator<ChangeUserPasswordCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new ChangeUserPasswordHandler(userRepository, tokenRepository, validator, hashService);

            var res = await handler.Handle(command, default);

            await tokenRepository.Received(2).UpdateAsync(Arg.Any<RefreshToken>());
            await userRepository.Received(1).UpdateAsync(user);

            Assert.True(res.IsSuccess);
            Assert.All(tokens, t => Assert.False(t.IsValid));
            Assert.Equal(NewHash, user.PasswordHash);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_InvalidRequest_Failure()
        {

            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                CurrentPassword = "password",
                NewPassword = "new password"
            };

            var userRepository = Substitute.For<IUserRepository>();

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            var hashService = Substitute.For<IHashService>();

            var validator = Substitute.For<IValidator<ChangeUserPasswordCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new ChangeUserPasswordHandler(userRepository, tokenRepository, validator, hashService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_UserNotFound_Failure()
        {

            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                CurrentPassword = "password",
                NewPassword = "new password"
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByIdAsync(command.UserId, Arg.Any<Func<User, User>>()).ReturnsNull();

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            var hashService = Substitute.For<IHashService>();

            var validator = Substitute.For<IValidator<ChangeUserPasswordCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new ChangeUserPasswordHandler(userRepository, tokenRepository, validator, hashService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_CurrentPasswordDoesNotMatchHash_Failure()
        {
            var user = new User
            {
                PasswordHash = "hash"
            };

            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                CurrentPassword = "password",
                NewPassword = "new password"
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByIdAsync(command.UserId, Arg.Any<Func<User, User>>()).Returns(user);

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            var hashService = Substitute.For<IHashService>();

            hashService.CompareStringWithHashAsync(command.CurrentPassword, user.PasswordHash).Returns(false);

            var validator = Substitute.For<IValidator<ChangeUserPasswordCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new ChangeUserPasswordHandler(userRepository, tokenRepository, validator, hashService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }
    }
}
