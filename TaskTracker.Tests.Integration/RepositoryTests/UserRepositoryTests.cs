using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Tests.Integration.RepositoryTests
{
    public class UserRepositoryTests : DatabaseTest
    {
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _repository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesProperEntity()
        {
            _dbContext.Users.AddRange(FakeDataFactory.GenerateUsers(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.First();

            await _repository.DeleteByIdAsync(user.Id);

            Assert.DoesNotContain(_dbContext.Users, u => u.Id == user.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_UserDoesNotExists_ExceptionThrown()
        {
            _dbContext.Users.AddRange(FakeDataFactory.GenerateUsers(5));
            _dbContext.SaveChanges();

            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repository.DeleteByIdAsync(2137));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProperUser()
        {
            _dbContext.Users.AddRange(FakeDataFactory.GenerateUsers(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.First();

            var model = await _repository.GetByIdAsync(user.Id);

            Assert.Equal(user.Id, model.Id);
            Assert.Equal(user.FirstName, model.FirstName);
            Assert.Equal(user.Email, model.Email);
            Assert.Equal(user.LastName, model.LastName);
        }

        [Fact]
        public async Task GetByIdAsync_WithCustomSelector_ReturnsProperValue()
        {
            _dbContext.Users.AddRange(FakeDataFactory.GenerateUsers(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.First();

            var email = await _repository.GetByIdAsync(user.Id, u => u.Email);

            Assert.Equal(user.Email, email);
        }

        [Fact]
        public async Task AddAsync_AddsEntity()
        {
            var user = new User
            {
                Email = "email",
                FirstName = "fname",
                LastName = "lname",
                PasswordHash = "hash",
            };

            var newId = await _repository.AddAsync(user);

            var createdUser = _dbContext.Users.First(u => u.Id == newId);

            Assert.Equal(user.Email, createdUser.Email);
            Assert.Equal(user.FirstName, createdUser.FirstName);
            Assert.Equal(user.LastName, createdUser.LastName);
            Assert.Equal(user.PasswordHash, createdUser.PasswordHash);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsCorrectUser()
        {
            _dbContext.Users.AddRange(FakeDataFactory.GenerateUsers(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.First();

            var result = await _repository.GetByEmailAsync(user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProperUser()
        {
            _dbContext.Users.AddRange(FakeDataFactory.GenerateUsers(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.First();

            user.Email = "newemail";

            await _repository.UpdateAsync(user);

            Assert.Equal(_dbContext.Users.First(x => x.Id == user.Id).Email, user.Email);
        }

        [Fact]
        public async Task GetAsync_SearchEmail_ReturnsProperEntities()
        {
            var users = FakeDataFactory.GenerateUsers(5);

            users.AddRange(new User[]
            {
                new User { Email = "email@email.com", FirstName = "fname", LastName = "lname", PasswordHash = "hash" },
                new User { Email = "email2@email.com", FirstName = "fname", LastName = "lname", PasswordHash = "hash" },
                new User { Email = "email3@email.com", FirstName = "fname", LastName = "lname", PasswordHash = "hash" },
            });

            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            var request = new GetUserRequest
            {
                SearchEmail = "email"
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(users.Where(x => x.Email.StartsWith(request.SearchEmail)).Select(x => x.Id), res.Select(x => x.Id));
        }
    }
}
