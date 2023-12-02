using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Application;
using TaskTracker.Application.Authorization.Exception;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class RefreshTokenCommandTests
    {
        [Fact]
        public async Task RefreshTokenCommand_Success()
        {
            var currentToken = new RefreshToken()
            {
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(1),
                IsValid = true,
                Token = "oldtoken",
                User = new User
                {
                    Id = 1,
                    Email = "email",
                    FirstName = "name",
                    LastName = "lname",
                },
                UserId = 1,
            };

            var tokens = new List<RefreshToken>();

            var command = new RefreshTokenCommand
            {
                AccessToken = "oldaccess",
                RefreshToken = currentToken.Token
            };

            const string NewAccessToken = "acc_token";
            const string NewRefreshToken = "ref_token";

            var tokenService = Substitute.For<ITokenService>();
            tokenService.GenerateAccessTokenAsync(currentToken.User).Returns(NewAccessToken);
            tokenService.GenerateRefreshTokenAsync().Returns(NewRefreshToken);
            tokenService.GetUserIdFromAccessTokenAsync(command.AccessToken).Returns(currentToken.User.Id);

            var tokenFactory = Substitute.For<IRefreshTokenFactory>();
            tokenFactory.Create(currentToken.User.Id, NewRefreshToken).Returns(info => new RefreshToken
            {
                UserId = info.Arg<long>(),
                Token = info.Arg<string>()
            });

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();
            tokenRepository.AddAsync(Arg.Any<RefreshToken>())
                .Returns(0L)
                .AndDoes(info => tokens.Add(info.Arg<RefreshToken>()));

            tokenRepository.GetTokenAsync(NewRefreshToken).ReturnsNull();
            tokenRepository.GetTokenAsync(currentToken.Token).Returns(currentToken);

            var handler = new RefreshTokenHandler(tokenRepository, tokenService, tokenFactory);

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
            Assert.False(currentToken.IsValid);
            Assert.Equal(NewAccessToken, res.Data.AccessToken);
            Assert.Equal(NewRefreshToken, res.Data.RefreshToken);
            Assert.Equal(currentToken.User.Email, res.Data.UserData.Email);
            Assert.Equal(currentToken.User.LastName, res.Data.UserData.LastName);
            Assert.Equal(currentToken.User.FirstName, res.Data.UserData.FirstName);
            Assert.Equal(currentToken.User.Id, res.Data.UserData.Id);
            Assert.Contains(tokens, x => x.Token == res.Data.RefreshToken && x.UserId == res.Data.UserData.Id);
        }

        [Fact]
        public async Task RefreshTokenCommand_TokenNotFound_Failure()
        {
            var command = new RefreshTokenCommand
            {
                AccessToken = "oldaccess",
                RefreshToken = "refresh"
            };

            var tokenService = Substitute.For<ITokenService>();

            var tokenFactory = Substitute.For<IRefreshTokenFactory>();

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            tokenRepository.GetTokenAsync(Arg.Any<string>()).ReturnsNull();

            var handler = new RefreshTokenHandler(tokenRepository, tokenService, tokenFactory);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task RefreshTokenCommand_InvalidAccessToken_Failure()
        {
            var currentToken = new RefreshToken()
            {
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(1),
                IsValid = true,
                Token = "oldtoken",
                User = new User
                {
                    Id = 1,
                    Email = "email",
                    FirstName = "name",
                    LastName = "lname",
                },
                UserId = 1,
            };

            var command = new RefreshTokenCommand
            {
                AccessToken = "oldaccess",
                RefreshToken = currentToken.Token
            };

            var tokenService = Substitute.For<ITokenService>();

            tokenService.GetUserIdFromAccessTokenAsync(command.AccessToken).Throws<InvalidTokenException>();

            var tokenFactory = Substitute.For<IRefreshTokenFactory>();

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            tokenRepository.GetTokenAsync(currentToken.Token).Returns(currentToken);

            var handler = new RefreshTokenHandler(tokenRepository, tokenService, tokenFactory);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task RefreshTokenCommand_AccessTokenUserId_And_RefreshTokenUserId_NotMatching_Failure()
        {
            var currentToken = new RefreshToken()
            {
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(1),
                IsValid = true,
                Token = "oldtoken",
                User = new User
                {
                    Id = 1,
                    Email = "email",
                    FirstName = "name",
                    LastName = "lname",
                },
                UserId = 1,
            };

            var command = new RefreshTokenCommand
            {
                AccessToken = "oldaccess",
                RefreshToken = currentToken.Token
            };

            var tokenService = Substitute.For<ITokenService>();
            tokenService.GetUserIdFromAccessTokenAsync(command.AccessToken).Returns(2137);

            var tokenFactory = Substitute.For<IRefreshTokenFactory>();

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            tokenRepository.GetTokenAsync(currentToken.Token).Returns(currentToken);

            var handler = new RefreshTokenHandler(tokenRepository, tokenService, tokenFactory);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task InvalidateRefreshTokenCommand_TokenDoesNotExist_Success()
        {
            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            tokenRepository.GetTokenAsync(Arg.Any<string>()).ReturnsNull();

            var handler = new InvalidateRefreshTokenHandler(tokenRepository);

            var command = new InvalidateRefreshTokenCommand { RefreshToken = "token" };

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task InvalidateRefreshTokenCommand_Success()
        {
            var token = new RefreshToken
            {
                IsValid = true,
            };

            var tokenRepository = Substitute.For<IRefreshTokenRepository>();

            tokenRepository.GetTokenAsync(Arg.Any<string>()).Returns(token);

            var handler = new InvalidateRefreshTokenHandler(tokenRepository);

            var command = new InvalidateRefreshTokenCommand { RefreshToken = "token" };

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
            Assert.False(token.IsValid);
        }
    }
}
