using TaskTracker.Application.Command;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Application;
using NSubstitute;
using TaskTracker.Model.TaskStatusGroup.Request;
using TaskTracker.Model.UserTaskStatus.Request;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute.ExceptionExtensions;
using TaskTracker.Database.Exception;
using NSubstitute.ReturnsExtensions;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class TaskStatusGroupCommandTests
    {
        [Fact]
        public async Task AddTaskStatusGroupCommand_AddsGroupAndDefaultStatuses()
        {
            var command = new AddTaskStatusGroupCommand
            {
                Name = "Name"
            };

            var groups = new List<TaskStatusGroup>();
            const long GroupId = 1;
            var groupRepository = Substitute.For<ITaskStatusGroupRepository>();

            groupRepository.AddAsync(Arg.Any<TaskStatusGroup>()).Returns(GroupId)
                .AndDoes(info => groups.Add(info.Arg<TaskStatusGroup>()));

            var statuses = new List<UserTaskStatus>();
            var statusRepository = Substitute.For<IUserTaskStatusRepository>();
            statusRepository.AddAsync(Arg.Any<UserTaskStatus[]>()).Returns(Task.CompletedTask)
                .AndDoes(info => statuses.AddRange(info.Arg<UserTaskStatus[]>()));

            var groupFactory = Substitute.For<ITaskStatusGroupFactory>();
            groupFactory.Create(Arg.Any<AddTaskStatusGroupRequest>(), false).Returns(info => new TaskStatusGroup
            {
                Name = info.Arg<AddTaskStatusGroupRequest>().Name
            });

            var statusFactory = Substitute.For<IUserTaskStatusFactory>();
            statusFactory.Create(Arg.Any<AddUserTaskStatusRequest>(), true).Returns(info => new UserTaskStatus
            {
                Name = info.Arg<AddUserTaskStatusRequest>().Name,
                Index = info.Arg<AddUserTaskStatusRequest>().Index,
                GroupId = info.Arg<AddUserTaskStatusRequest>().GroupId
            });

            var validator = Substitute.For<IValidator<AddTaskStatusGroupCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new AddTaskStatusGroupHandler(groupRepository, groupFactory, validator, statusRepository, statusFactory);

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
            Assert.Equal(GroupId, res.NewEntityId);
            Assert.Contains(groups, x => x.Name == command.Name);
            Assert.Contains(statuses, x => x.Index == 0 && x.Name == "Backlog");
            Assert.Contains(statuses, x => x.Index == 21 && x.Name == "Closed");
            Assert.All(statuses, status => Assert.Equal(GroupId, status.GroupId));
        }

        [Fact]
        public async Task AddTaskStatusGroupCommand_InvalidRequest_Failure()
        {
            var command = new AddTaskStatusGroupCommand
            {
                Name = "Name"
            };

            var groupRepository = Substitute.For<ITaskStatusGroupRepository>();
            var statusRepository = Substitute.For<IUserTaskStatusRepository>();

            var groupFactory = Substitute.For<ITaskStatusGroupFactory>();

            var statusFactory = Substitute.For<IUserTaskStatusFactory>();

            var validator = Substitute.For<IValidator<AddTaskStatusGroupCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddTaskStatusGroupHandler(groupRepository, groupFactory, validator, statusRepository, statusFactory);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task DeleteTaskStatusGroupByIdCommand_Success()
        {
            var command = new DeleteTaskStatusGroupByIdCommand
            {
                Id = 1
            };

            var repository = Substitute.For<ITaskStatusGroupRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<TaskStatusGroup, bool>>()).Returns(false);

            var handler = new DeleteTaskStatusGroupByIdHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(1).DeleteByIdAsync(command.Id);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteTaskStatusGroupByIdCommand_DefaultGroup_Failure()
        {
            var command = new DeleteTaskStatusGroupByIdCommand
            {
                Id = 1
            };

            var repository = Substitute.For<ITaskStatusGroupRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<TaskStatusGroup, bool>>()).Returns(true);

            var handler = new DeleteTaskStatusGroupByIdHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(0).DeleteByIdAsync(command.Id);

            Assert.False(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteTaskStatusGroupByIdCommand_EntityNotFoundException_Failure()
        {
            var command = new DeleteTaskStatusGroupByIdCommand
            {
                Id = 1
            };

            var repository = Substitute.For<ITaskStatusGroupRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<TaskStatusGroup, bool>>()).Returns(false);
            repository.DeleteByIdAsync(command.Id).Throws(new EntityNotFoundException("ex"));

            var handler = new DeleteTaskStatusGroupByIdHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }

        [Fact]
        public async Task GetTaskStatusGroupByIdQuery_ReturnsModel()
        {
            var query = new GetTaskStatusGroupByIdQuery { Id = 1 };

            var repository = Substitute.For<ITaskStatusGroupRepository>();

            var model = new TaskStatusGroupModel();
            repository.GetByIdAsync(query.Id).Returns(model);

            var handler = new GetTaskStatusGroupByIdHandler(repository);

            var res = await handler.Handle(query, default);

            Assert.Equal(model, res);
        }

        [Fact]
        public async Task GetTaskStatusGroupQuery_ReturnsListOfModels()
        {
            var query = new GetTaskStatusGroupQuery();

            var repository = Substitute.For<ITaskStatusGroupRepository>();

            var models = new List<TaskStatusGroupModel>
            {
                new TaskStatusGroupModel(),
                new TaskStatusGroupModel()
            };

            repository.GetAsync(query).Returns(models);

            var handler = new GetTaskStatusGroupHandler(repository);

            var res = await handler.Handle(query, default);

            Assert.Equivalent(models, res);
        }

        [Fact]
        public async Task UpdateTaskStatusGroupCommand_GroupUpdated()
        {
            var command = new UpdateTaskStatusGroupCommand
            {
                Id = 1,
                Name = "A",
            };

            var group = new TaskStatusGroup
            {
                Name = "b"
            };

            var repository = Substitute.For<ITaskStatusGroupRepository>();
            repository.GetByIdAsync(command.Id, Arg.Any<Func<TaskStatusGroup, TaskStatusGroup>>()).Returns(group);
            
            var validator = Substitute.For<IValidator<UpdateTaskStatusGroupCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new UpdateTaskStatusGroupHandler(repository, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(group);

            Assert.True(res.IsSuccess);
            Assert.Equal(command.Name, group.Name);
        }

        [Fact]
        public async Task UpdateTaskStatusGroupCommand_InvalidRequest_Failure()
        {
            var command = new UpdateTaskStatusGroupCommand
            {
                Id = 1,
                Name = "A",
            };

            var repository = Substitute.For<ITaskStatusGroupRepository>();

            var validator = Substitute.For<IValidator<UpdateTaskStatusGroupCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateTaskStatusGroupHandler(repository, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(0).UpdateAsync(Arg.Any<TaskStatusGroup>());

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateTaskStatusCommand_EntityNotFound_Failure()
        {
            var command = new UpdateTaskStatusGroupCommand
            {
                Id = 1,
                Name = "A",
            };

            var repository = Substitute.For<ITaskStatusGroupRepository>();
            repository.GetByIdAsync(command.Id, Arg.Any<Func<TaskStatusGroup, TaskStatusGroup>>()).ReturnsNull();

            var validator = Substitute.For<IValidator<UpdateTaskStatusGroupCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new UpdateTaskStatusGroupHandler(repository, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }
    }
}
