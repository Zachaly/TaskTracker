using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System.Threading.Tasks;
using TaskTracker.Application;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class UserTaskCommandTests
    {
        [Fact]
        public async Task GetUserTaskQuery_ReturnsTasks()
        {
            var request = new GetUserTaskQuery();
            var tasks = new List<UserTaskModel>
            {
                new UserTaskModel { Id = 1 },
                new UserTaskModel { Id = 2 },
                new UserTaskModel { Id = 3 },
                new UserTaskModel { Id = 4 },
                new UserTaskModel { Id = 5 },
            };

            var repository = Substitute.For<IUserTaskRepository>();

            repository.GetAsync(request).Returns(tasks);

            var handler = new GetUserTaskHandler(repository);

            var res = await handler.Handle(request, default);

            Assert.Equal(tasks, res);
        }

        [Fact]
        public async Task GetUserTaskByIdQuery_ReturnsTask()
        {
            var task = new UserTaskModel();
            var request = new GetUserTaskByIdQuery() { Id = 1 };

            var repository = Substitute.For<IUserTaskRepository>();

            repository.GetByIdAsync(request.Id).Returns(task);

            var handler = new GetUserTaskByIdHandler(repository);
            var res = await handler.Handle(request, default);

            Assert.Equal(task, res);
        }

        [Fact]
        public async Task AddUserTaskCommand_Success()
        {
            var tasks = new List<UserTask>();

            var request = new AddUserTaskCommand
            {
                CreatorId = 1,
            };

            var repository = Substitute.For<IUserTaskRepository>();

            const long NewId = 2;

            repository.AddAsync(Arg.Any<UserTask>()).Returns(NewId).AndDoes(info => tasks.Add(info.Arg<UserTask>()));

            var factory = Substitute.For<IUserTaskFactory>();

            factory.Create(request).Returns(new UserTask { CreatorId = request.CreatorId });

            var validator = Substitute.For<IValidator<AddUserTaskCommand>>();
            validator.ValidateAsync(request).Returns(new ValidationResult());

            var handler = new AddUserTaskHandler(factory, repository, validator);

            var res = await handler.Handle(request, default);

            Assert.True(res.IsSuccess);
            Assert.Contains(tasks, x => x.CreatorId == request.CreatorId);
            Assert.Equal(NewId, res.NewEntityId);
        }

        [Fact]
        public async Task AddUserTaskCommand_InvalidRequest_Failure()
        {
            var request = new AddUserTaskCommand();

            var repository = Substitute.For<IUserTaskRepository>();

            var factory = Substitute.For<IUserTaskFactory>();

            var validator = Substitute.For<IValidator<AddUserTaskCommand>>();
            validator.ValidateAsync(request).
                Returns(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("prop", "invalidProp") }));

            var handler = new AddUserTaskHandler(factory, repository, validator);

            var res = await handler.Handle(request, default);

            Assert.False(res.IsSuccess);
            Assert.Null(res.NewEntityId);
        }
    }
}
