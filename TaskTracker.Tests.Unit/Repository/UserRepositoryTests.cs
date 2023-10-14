using Bogus;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Database;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Unit.Repository
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _repository;
        private readonly ApplicationDbContext _dbContext;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);

            _repository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesProperEntity()
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20));

            _dbContext.Users.AddRange(faker.Generate(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.Last();

            await _repository.DeleteByIdAsync(user.Id);

            Assert.DoesNotContain(_dbContext.Users, u => u.Id == user.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_UserDoesNotExists_ExceptionThrown()
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20));

            _dbContext.Users.AddRange(faker.Generate(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.Last();

            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repository.DeleteByIdAsync(user.Id + 1));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProperUser()
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20));

            _dbContext.Users.AddRange(faker.Generate(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.Last();

            var model = await _repository.GetByIdAsync(user.Id);

            Assert.Equal(user.Id, model.Id);
            Assert.Equal(user.FirstName, model.FirstName);
            Assert.Equal(user.Email, model.Email);
            Assert.Equal(user.LastName, model.LastName);
        }

        [Fact]
        public async Task GetByIdAsync_WithCustomSelector_ReturnsProperValue()
        {
            var faker = new Faker<User>()
               .RuleFor(u => u.Email, f => f.Person.Email)
               .RuleFor(u => u.FirstName, f => f.Person.FirstName)
               .RuleFor(u => u.LastName, f => f.Person.LastName)
               .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20));

            _dbContext.Users.AddRange(faker.Generate(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.Last();

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
            var faker = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20));

            _dbContext.Users.AddRange(faker.Generate(5));
            _dbContext.SaveChanges();

            var user = _dbContext.Users.Last();

            var result = await _repository.GetByEmailAsync(user.Email);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProperUser()
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20));

            _dbContext.Users.AddRange(faker.Generate(5));
            _dbContext.SaveChanges();


            var user = _dbContext.Users.First();

            user.Email = "newemail";

            await _repository.UpdateAsync(user);

            Assert.Equal(_dbContext.Users.First(x => x.Id == user.Id).Email, user.Email);
        }
    }
}
