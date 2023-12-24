using TaskTracker.Database.Repository;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Tests.Integration.RepositoryTests
{
    public class TaskListRepositoryTests : DatabaseTest
    {
        private readonly TaskListRepository _repository;

        public TaskListRepositoryTests() : base()
        {
            _repository = new TaskListRepository(_dbContext);
        }

        [Fact]
        public async Task GetAsync_ReturnsProperEntities()
        {
            var users = FakeDataFactory.GenerateUsers(2);

            _dbContext.Users.AddRange(users);

            _dbContext.SaveChanges();

            var userIds = _dbContext.Users.Select(x => x.Id).ToList();

            var group = FakeDataFactory.GenerateTaskStatusGroups(1, userIds.First()).First();

            _dbContext.TaskStatusGroups.Add(group);
            _dbContext.SaveChanges();

            var space = FakeDataFactory.GenerateUserSpaces(1, userIds.First(), group.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            _dbContext.TaskLists.AddRange(userIds.SelectMany(id => FakeDataFactory.GenerateTaskLists(5, id, group.Id, space.Id)));
            _dbContext.SaveChanges();

            var userId = _dbContext.Users.First().Id;

            var request = new GetTaskListRequest
            {
                CreatorId = userId,
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(_dbContext.TaskLists.Where(l => l.CreatorId == userId).Select(x => x.Id),
                res.Select(x => x.Id));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProperEntity()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var group = FakeDataFactory.GenerateTaskStatusGroups(1, user.Id).First();

            _dbContext.TaskStatusGroups.Add(group);
            _dbContext.SaveChanges();

            var space = FakeDataFactory.GenerateUserSpaces(1, user.Id, group.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            _dbContext.TaskLists.AddRange(FakeDataFactory.GenerateTaskLists(5, user.Id, group.Id, space.Id));
            _dbContext.SaveChanges();

            var updatedList = _dbContext.TaskLists.AsEnumerable().First();

            await _repository.UpdateAsync(updatedList);

            var list = _dbContext.TaskLists.First(x => x.Id == updatedList.Id);

            Assert.Equal(updatedList.Id, list.Id);
            Assert.Equal(updatedList.Color, list.Color);
            Assert.Equal(updatedList.Description, list.Description);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperEntityAndChildren()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var group = FakeDataFactory.GenerateTaskStatusGroups(1, user.Id).First();

            _dbContext.TaskStatusGroups.Add(group);
            _dbContext.SaveChanges();

            var status = FakeDataFactory.GenerateTaskStatuses(1, group.Id).First();

            _dbContext.UserTaskStatuses.Add(status);
            _dbContext.SaveChanges();

            var space = FakeDataFactory.GenerateUserSpaces(1, user.Id, group.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var lists = FakeDataFactory.GenerateTaskLists(2, user.Id, group.Id, space.Id);

            _dbContext.TaskLists.AddRange(lists);

            _dbContext.SaveChanges();

            var listIds = _dbContext.TaskLists.Select(x => x.Id).ToList();

            _dbContext.Tasks.AddRange(listIds.SelectMany(id => FakeDataFactory.GenerateUserTasks(5, user.Id, id, status.Id)));
            _dbContext.SaveChanges();

            var deletedId = listIds.Last();

            await _repository.DeleteByIdAsync(deletedId);

            Assert.DoesNotContain(_dbContext.TaskLists, x => x.Id == deletedId);
            Assert.DoesNotContain(_dbContext.Tasks, x => x.ListId == deletedId);
            Assert.Contains(_dbContext.Users, x => x.Id == user.Id);
        }
    }
}
