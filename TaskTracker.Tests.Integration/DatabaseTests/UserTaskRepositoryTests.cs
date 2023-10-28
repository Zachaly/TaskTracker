using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Tests.Integration.DatabaseTests
{
    public class UserTaskRepositoryTests : DatabaseTest
    {
        private readonly UserTaskRepository _repository;

        public UserTaskRepositoryTests()
        {
            _repository = new UserTaskRepository(_dbContext);
        }

        [Fact]
        public async Task GetAsync_CreatorIdSpecified_ReturnsProperEntities()
        {
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                Email = "email",
                PasswordHash = "hash",
            };

            _dbContext.Users.Add(user);
            _dbContext.Users.Add(new User
            {
                FirstName = "fname2",
                LastName = "lname2",
                Email = "email2",
                PasswordHash = "hash2"
            });
            _dbContext.SaveChanges();

            var tasks = new List<UserTask>
            {
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id },
            };

            _dbContext.Tasks.AddRange(tasks);

            _dbContext.SaveChanges();

            var request = new GetUserTaskRequest
            {
                CreatorId = user.Id
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(_dbContext.Tasks.Where(x => x.CreatorId == user.Id).Select(x => x.Id), res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetAsync_MinCreationTimestampSpecified_ReturnsProperEntities()
        {
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                Email = "email",
                PasswordHash = "hash",
            };

            _dbContext.Users.Add(user);
            _dbContext.Users.Add(new User
            {
                FirstName = "fname2",
                LastName = "lname2",
                Email = "email2",
                PasswordHash = "hash2"
            });
            _dbContext.SaveChanges();

            const long Timestamp = 3;

            var tasks = new List<UserTask>
            {
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, CreationTimestamp = Timestamp },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2, CreationTimestamp = 1 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2, CreationTimestamp = Timestamp + 1 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, CreationTimestamp = 2 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2, CreationTimestamp = Timestamp + 2 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, CreationTimestamp = 1 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, CreationTimestamp = Timestamp },
            };

            _dbContext.Tasks.AddRange(tasks);

            _dbContext.SaveChanges();

            var request = new GetUserTaskRequest
            {
                MinCreationTimestamp = Timestamp,
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(_dbContext.Tasks.Where(x => x.CreationTimestamp >= Timestamp).Select(x => x.Id), res.Select(x => x.Id), true);
        }

        [Fact]
        public async Task GetAsync_MaxDueTimestampSpecified_ReturnsProperEntities()
        {
            var user = new User
            {
                FirstName = "fname",
                LastName = "lname",
                Email = "email",
                PasswordHash = "hash",
            };

            _dbContext.Users.Add(user);
            _dbContext.Users.Add(new User
            {
                FirstName = "fname2",
                LastName = "lname2",
                Email = "email2",
                PasswordHash = "hash2"
            });
            _dbContext.SaveChanges();

            const long Timestamp = 3;

            var tasks = new List<UserTask>
            {
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, DueTimestamp = Timestamp },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2, DueTimestamp = 1 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2, DueTimestamp = Timestamp + 1 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, DueTimestamp = 2 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = 2, DueTimestamp = Timestamp + 2 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, DueTimestamp = 1 },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, DueTimestamp = Timestamp, },
                new UserTask { Description = "desc", Title = "tit", CreatorId = user.Id, DueTimestamp = null, },
            };

            _dbContext.Tasks.AddRange(tasks);

            _dbContext.SaveChanges();

            var request = new GetUserTaskRequest
            {
                MaxDueTimestamp = Timestamp,
            };

            var res = (await _repository.GetAsync(request)).ToList();

            var t = _dbContext.Tasks.Where(x => x.DueTimestamp <= Timestamp).ToList();

            Assert.Equivalent(_dbContext.Tasks.Where(x => x.DueTimestamp <= Timestamp).Select(x => x.Id), res.Select(x => x.Id), true);
        }
    }
}
