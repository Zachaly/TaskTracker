using TaskTracker.Database.Repository;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Tests.Integration.RepositoryTests
{
    public class TaskStatusGroupRepositoryTests : DatabaseTest
    {
        private readonly TaskStatusGroupRepository _repository;

        public TaskStatusGroupRepositoryTests() : base()
        {
            _repository = new TaskStatusGroupRepository(_dbContext);
        }

        [Fact]
        public async Task GetAsync_ReturnsCorrectEntities()
        {
            var users = FakeDataFactory.GenerateUsers(2);

            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            var user1 = users.First();
            var user2 = users.Last();

            var user1Groups = FakeDataFactory.GenerateTaskStatusGroups(5, user1.Id);
            var user2Groups = FakeDataFactory.GenerateTaskStatusGroups(5, user2.Id);

            _dbContext.TaskStatusGroups.AddRange(user1Groups);
            _dbContext.TaskStatusGroups.AddRange(user2Groups);

            _dbContext.SaveChanges();

            var request = new GetTaskStatusGroupRequest
            {
                UserId = user1.Id,
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(user1Groups.Select(x => x.Id), res.Select(x => x.Id));
            Assert.All(res, g => Assert.Empty(g.Statuses));
        }

        [Fact]
        public async Task GetByIdAsync_StatusesJoined()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var group = FakeDataFactory.GenerateTaskStatusGroups(1, user.Id).First();

            group.Statuses = FakeDataFactory.GenerateTaskStatuses(4, group.Id);

            _dbContext.TaskStatusGroups.Add(group);
            _dbContext.SaveChanges();

            var res = await _repository.GetByIdAsync(group.Id);

            Assert.Equivalent(group.Statuses.Select(x => x.Id), res.Statuses.Select(x => x.Id));
        }
    }
}
