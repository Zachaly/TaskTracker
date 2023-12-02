using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Integration.RepositoryTests
{
    public class RefreshTokenRepositoryTests : DatabaseTest
    {
        private RefreshTokenRepository _repository;

        public RefreshTokenRepositoryTests()
        {
            _repository = new RefreshTokenRepository(_dbContext);
        }

        [Fact]
        public async Task GetByTokenAsync_ReturnsToken()
        {
            var user = new User
            {
                Email = "email",
                FirstName = "name",
                LastName = "name",
                PasswordHash = "password",
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var refreshToken = new RefreshToken
            {
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                IsValid = true,
                UserId = user.Id,
                Token = "token"
            };

            var tokens = new List<RefreshToken>
            {
                new RefreshToken { IsValid = true, ExpiryDate = DateTime.UtcNow.AddDays(1), Token = "tok", UserId = user.Id },
                refreshToken,
                new RefreshToken { IsValid = false, ExpiryDate = DateTime.UtcNow.AddDays(-1), Token = "tok2", UserId = user.Id },
            };

            _dbContext.Set<RefreshToken>().AddRange(tokens);
            _dbContext.SaveChanges();

            var res = await _repository.GetTokenAsync(refreshToken.Token);

            Assert.NotNull(res);
            Assert.Equal(refreshToken.Token, res.Token);
            Assert.Equal(refreshToken.Id, res.Id);
            Assert.Equal(refreshToken.IsValid, res.IsValid);
            Assert.Equal(refreshToken.CreationDate, res.CreationDate);
            Assert.Equal(refreshToken.ExpiryDate, res.ExpiryDate);
            Assert.Equal(refreshToken.UserId, res.UserId);
        }

        [Fact]
        public async Task GetByTokenAsync_InvalidatedToken_ReturnsNull()
        {
            var user = new User
            {
                Email = "email",
                FirstName = "name",
                LastName = "name",
                PasswordHash = "password",
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var refreshToken = new RefreshToken
            {
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                IsValid = false,
                UserId = user.Id,
                Token = "token"
            };

            var tokens = new List<RefreshToken>
            {
                new RefreshToken { IsValid = true, ExpiryDate = DateTime.UtcNow.AddDays(1), Token = "tok", UserId = user.Id },
                refreshToken,
                new RefreshToken { IsValid = false, ExpiryDate = DateTime.UtcNow.AddDays(-1), Token = "tok2", UserId = user.Id },
            };

            _dbContext.Set<RefreshToken>().AddRange(tokens);
            _dbContext.SaveChanges();

            var res = await _repository.GetTokenAsync(refreshToken.Token);

            Assert.Null(res);
        }

        [Fact]
        public async Task GetByTokenAsync_ExpiredToken_ReturnsNull()
        {
            var user = new User
            {
                Email = "email",
                FirstName = "name",
                LastName = "name",
                PasswordHash = "password",
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var refreshToken = new RefreshToken
            {
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(-2),
                IsValid = true,
                UserId = user.Id,
                Token = "token"
            };

            var tokens = new List<RefreshToken>
            {
                new RefreshToken { IsValid = true, ExpiryDate = DateTime.UtcNow.AddDays(1), Token = "tok", UserId = user.Id },
                refreshToken,
                new RefreshToken { IsValid = false, ExpiryDate = DateTime.UtcNow.AddDays(-1), Token = "tok2", UserId = user.Id },
            };

            _dbContext.Set<RefreshToken>().AddRange(tokens);
            _dbContext.SaveChanges();

            var res = await _repository.GetTokenAsync(refreshToken.Token);

            Assert.Null(res);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProperEntity()
        {
            var user = new User
            {
                Email = "email",
                FirstName = "name",
                LastName = "name",
                PasswordHash = "password",
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var token = new RefreshToken
            {
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                IsValid = true,
                Token = "tok",
                UserId = user.Id
            };

            _dbContext.Set<RefreshToken>().Add(token);

            _dbContext.SaveChanges();

            token.IsValid = false;

            await _repository.UpdateAsync(token);

            Assert.Equal(token.IsValid, _dbContext.RefreshTokens.First(x => x.Id == token.Id).IsValid);
        }
    }
}
