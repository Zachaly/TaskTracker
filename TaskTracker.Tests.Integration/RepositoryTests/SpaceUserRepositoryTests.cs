using TaskTracker.Database.Repository;

namespace TaskTracker.Tests.Integration.RepositoryTests
{
    public class SpaceUserRepositoryTests : DatabaseTest
    {
        private readonly SpaceUserRepository _repository;

        public SpaceUserRepositoryTests() : base()
        {
            _repository = new SpaceUserRepository(_dbContext);
        }

        [Fact]
        public async Task DeleteByUserIdAndSpaceIdAsync_DeletesProperEntity()
        {
            var users = FakeDataFactory.GenerateUsers(5);

            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            var statusGroup = FakeDataFactory.GenerateTaskStatusGroups(1, users.First().Id).First();

            _dbContext.TaskStatusGroups.Add(statusGroup);
            _dbContext.SaveChanges();

            var spaces = users.Select(x => x.Id).SelectMany(id => FakeDataFactory.GenerateUserSpaces(2, id, statusGroup.Id));

            _dbContext.UserSpaces.AddRange(spaces);
            _dbContext.SaveChanges();

            var spaceUsers = _dbContext.UserSpaces.AsEnumerable()
                .Select(x => x.Id).SelectMany(id => FakeDataFactory.GenerateSpaceUsers(id, users.Select(x => x.Id)));

            _dbContext.SpaceUsers.AddRange(spaceUsers.ToList());
            _dbContext.SaveChanges();

            var deletedUser = _dbContext.SpaceUsers.First();

            await _repository.DeleteByUserIdAndSpaceIdAsync(deletedUser.UserId, deletedUser.SpaceId);

            Assert.DoesNotContain(_dbContext.SpaceUsers, user => user.SpaceId == deletedUser.SpaceId && user.UserId == deletedUser.UserId);
        }
    }
}
