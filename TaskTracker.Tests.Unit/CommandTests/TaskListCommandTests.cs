using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Application;
using TaskTracker.Application.Command;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class TaskListCommandTests
    {
        [Fact]
        public async Task AddTaskListCommand_Success()
        {
            var command = new AddTaskListCommand
            {
                CreatorId = 1
            };

            var lists = new List<TaskList>();
            const long NewId = 2;
            var repository = Substitute.For<ITaskListRepository>();
            repository.AddAsync(Arg.Any<TaskList>())
                .Returns(NewId)
                .AndDoes(info => lists.Add(info.Arg<TaskList>()));


            var factory = Substitute.For<ITaskListFactory>();

            factory.Create(command).Returns(info => new TaskList
            {
                CreatorId = info.Arg<AddTaskListCommand>().CreatorId
            });

            var validator = Substitute.For<IValidator<AddTaskListCommand>>();

            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new AddTaskListHandler(repository, factory, validator);

            var response = await handler.Handle(command, default);

            Assert.True(response.IsSuccess);
            Assert.Contains(lists, x => x.CreatorId == command.CreatorId);
            Assert.Equal(NewId, response.NewEntityId);
        }

        [Fact]
        public async Task AddTaskListCommand_InvalidRequest_Failure()
        {
            var command = new AddTaskListCommand
            {
                CreatorId = 1
            };
            var repository = Substitute.For<ITaskListRepository>();

            var factory = Substitute.For<ITaskListFactory>();

            var validator = Substitute.For<IValidator<AddTaskListCommand>>();

            validator.ValidateAsync(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "error")
            }));

            var handler = new AddTaskListHandler(repository, factory, validator);

            var response = await handler.Handle(command, default);

            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.ValidationErrors);
        }

        [Fact]
        public async Task DeleteTaskListByIdCommand_Success()
        {   
            var command = new DeleteTaskListByIdCommand { Id = 1 };
            var repository = Substitute.For<ITaskListRepository>();

            repository.DeleteByIdAsync(command.Id).Returns(Task.CompletedTask);
           
            var handler = new DeleteTaskListByIdHandler(repository);

            var response = await handler.Handle(command, default);

            await repository.Received(1).DeleteByIdAsync(command.Id);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task DeleteTaskListByIdCommand_EntityNotFound_Failure()
        {
            var command = new DeleteTaskListByIdCommand { Id = 1 };
            var repository = Substitute.For<ITaskListRepository>();

            repository.DeleteByIdAsync(command.Id).Throws(new EntityNotFoundException("ex"));

            var handler = new DeleteTaskListByIdHandler(repository);

            var response = await handler.Handle(command, default);

            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.Error);
        }

        [Fact]
        public async Task GetTaskListQuery_ReturnsTasks()
        {
            var command = new GetTaskListQuery();

            var tasks = new List<TaskListModel>
            {
                new TaskListModel { Id = 1 },
                new TaskListModel { Id = 2 },
                new TaskListModel { Id = 3 }
            };

            var repository = Substitute.For<ITaskListRepository>();

            repository.GetAsync(command).Returns(tasks);

            var handler = new GetTaskListHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equivalent(tasks, res);
        }

        [Fact]
        public async Task GetTaskListByIdQuery_ReturnsTask()
        {
            var command = new GetTaskListByIdQuery { Id = 1 };

            var task = new TaskListModel { Id = 1 };

            var repository = Substitute.For<ITaskListRepository>();

            repository.GetByIdAsync(command.Id).Returns(task);

            var handler = new GetTaskListByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.Equal(task, res);
        }

        [Fact]
        public async Task UpdateTaskListCommand_Success()
        {
            var list = new TaskList 
            {
                Id = 1,
                Description = "desc",
                Title = "title",
                Color = null
            };

            var request = new UpdateTaskListCommand
            {
                Color = "color",
                Title = "new title",
                Description = null,
                Id = list.Id
            };

            var repository = Substitute.For<ITaskListRepository>();

            repository.GetByIdAsync(list.Id, Arg.Any<Func<TaskList, TaskList>>()).Returns(list);

            repository.UpdateAsync(list).Returns(Task.CompletedTask);

            var validator = Substitute.For<IValidator<UpdateTaskListCommand>>();

            validator.ValidateAsync(request).Returns(new ValidationResult());

            var handler = new UpdateTaskListHandler(repository, validator);

            var response = await handler.Handle(request, default);

            Assert.True(response.IsSuccess);
            Assert.Equal(request.Title, list.Title);
            Assert.Equal(request.Color, list.Color);
            Assert.NotEqual(request.Description, list.Description);
        }

        [Fact]
        public async Task UpdateTaskListCommand_InvalidRequest_Failure()
        {
            var request = new UpdateTaskListCommand();
            var repository = Substitute.For<ITaskListRepository>();

            var validator = Substitute.For<IValidator<UpdateTaskListCommand>>();

            validator.ValidateAsync(request).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateTaskListHandler(repository, validator);

            var response = await handler.Handle(request, default);

            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task UpdateTaskListCommand_EntityNotFound_Failure()
        {
            var request = new UpdateTaskListCommand
            {
                Id = 1
            };

            var repository = Substitute.For<ITaskListRepository>();

            repository.GetByIdAsync(request.Id, Arg.Any<Func<TaskList, TaskList>>()).ReturnsNull();

            var validator = Substitute.For<IValidator<UpdateTaskListCommand>>();

            validator.ValidateAsync(request).Returns(new ValidationResult());

            var handler = new UpdateTaskListHandler(repository, validator);

            var response = await handler.Handle(request, default);

            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.Error);
        }
    }
}
