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
using TaskTracker.Model.UserTaskStatus;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class UserTaskStatusCommandTests
    {
        [Fact]
        public async Task AddUserTaskStatusCommand_AddsNewStatus()
        {
            var command = new AddUserTaskStatusCommand
            {
                Index = 1
            };

            var statuses = new List<UserTaskStatus>();

            const long NewId = 2;

            var repository = Substitute.For<IUserTaskStatusRepository>();
            repository.AddAsync(Arg.Any<UserTaskStatus>()).Returns(NewId)
                .AndDoes(info => statuses.Add(info.Arg<UserTaskStatus>()));

            var factory = Substitute.For<IUserTaskStatusFactory>();
            factory.Create(command).Returns(info => new UserTaskStatus
            {
                Index = info.Arg<AddUserTaskStatusCommand>().Index
            });

            var validator = Substitute.For<IValidator<AddUserTaskStatusCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new AddUserTaskStatusHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
            Assert.Equal(NewId, res.NewEntityId);
            Assert.Contains(statuses, x => x.Index == command.Index);
        }

        [Fact]
        public async Task AddUserTaskStatusCommand_InvalidRequest_Failure()
        {
            var command = new AddUserTaskStatusCommand();

            var repository = Substitute.For<IUserTaskStatusRepository>();

            var factory = Substitute.For<IUserTaskStatusFactory>();

            var validator = Substitute.For<IValidator<AddUserTaskStatusCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddUserTaskStatusHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task DeleteUserTaskStatusByIdCommand_StatusDeleted()
        {
            var command = new DeleteUserTaskStatusByIdCommand { Id = 1 };

            var repository = Substitute.For<IUserTaskStatusRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<UserTaskStatus, bool>>()).Returns(false);

            var handler = new DeleteUserTaskStatusByIdHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(1).DeleteByIdAsync(command.Id);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteUserTaskStatusByIdCommand_DefaultTask_Failure()
        {
            var command = new DeleteUserTaskStatusByIdCommand { Id = 1 };

            var repository = Substitute.For<IUserTaskStatusRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<UserTaskStatus, bool>>()).Returns(true);

            var handler = new DeleteUserTaskStatusByIdHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(0).DeleteByIdAsync(command.Id);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task DeleteUserTaskByIdCommand_EntityNotFound_Failure()
        {
            var command = new DeleteUserTaskStatusByIdCommand { Id = 1 };

            var repository = Substitute.For<IUserTaskStatusRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<UserTaskStatus, bool>>()).Returns(false);

            repository.DeleteByIdAsync(command.Id).Throws(new EntityNotFoundException("ex"));

            var handler = new DeleteUserTaskStatusByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task GetUserTaskStatusByIdQuery_ReturnsModel()
        {
            var query = new GetUserTaskStatusByIdQuery() { Id = 1 };

            var status = new UserTaskStatusModel();

            var repository = Substitute.For<IUserTaskStatusRepository>();
            repository.GetByIdAsync(query.Id).Returns(status);

            var handler = new GetUserTaskStatusByIdHandler(repository);

            var res = await handler.Handle(query, default);

            Assert.Equal(status, res);
        }

        [Fact]
        public async Task GetUserTaskStatusQuery_ReturnsListOfModels()
        {
            var query = new GetUserTaskStatusQuery();

            var list = new List<UserTaskStatusModel>
            {
                new UserTaskStatusModel(),
                new UserTaskStatusModel()
            };

            var repository = Substitute.For<IUserTaskStatusRepository>();

            repository.GetAsync(query).Returns(list);

            var handler = new GetUserTaskStatusHandler(repository);

            var res = await handler.Handle(query, default);

            Assert.Equivalent(list, res);
        }

        [Fact]
        public async Task UpdateUserTaskStatusCommand_UpdatesStatus()
        {
            var command = new UpdateUserTaskStatusCommand() { Id = 1, Index = 2 };

            var status = new UserTaskStatus { Index = 3 };

            var repository = Substitute.For<IUserTaskStatusRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<UserTaskStatus, UserTaskStatus>>()).Returns(status);

            var validator = Substitute.For<IValidator<UpdateUserTaskStatusCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new UpdateUserTaskStatusHandler(repository, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(status);

            Assert.True(res.IsSuccess);
            Assert.Equal(command.Index, status.Index);
        }

        [Fact]
        public async Task UpdateUserTaskStatusCommand_InvalidRequest_Failure()
        {
            var command = new UpdateUserTaskStatusCommand() { Id = 1, Index = 2 };
            var repository = Substitute.For<IUserTaskStatusRepository>();

            var validator = Substitute.For<IValidator<UpdateUserTaskStatusCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateUserTaskStatusHandler(repository, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateUserTaskStatusCommand_EntityNotFound_Failure()
        {
            var command = new UpdateUserTaskStatusCommand() { Id = 1, Index = 2 };

            var repository = Substitute.For<IUserTaskStatusRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<UserTaskStatus, UserTaskStatus>>()).ReturnsNull();

            var validator = Substitute.For<IValidator<UpdateUserTaskStatusCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new UpdateUserTaskStatusHandler(repository, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }
    }
}
