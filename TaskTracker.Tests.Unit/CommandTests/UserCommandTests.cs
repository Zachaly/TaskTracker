using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Application;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup.Request;
using TaskTracker.Model.User.Request;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class UserCommandTests
    {
        [Fact]
        public async Task RegisterCommand_CreatesNewUser_AndAddsDefaultStatusGroup()
        {
            var users = new List<User>();

            var userFactory = Substitute.For<IUserFactory>();
            userFactory.Create(Arg.Any<RegisterRequest>(), Arg.Any<string>()).Returns(arg => new User
            {
                Email = arg.Arg<RegisterRequest>().Email,
                PasswordHash = arg.Arg<string>()
            });

            const string PasswordHash = "hash";

            var hashService = Substitute.For<IHashService>();
            hashService.HashStringAsync(Arg.Any<string>()).Returns(PasswordHash);

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).ReturnsNull();

            const long NewId = 1;
            userRepository.AddAsync(Arg.Any<User>()).Returns(NewId).AndDoes(info =>
            {
                var user = info.Arg<User>();
                user.Id = NewId;
                users.Add(user);
            });

            const long StatusGroupId = 2;
            var statusGroups = new List<TaskStatusGroup>();
            var statuses = new List<UserTaskStatus>();

            var statusGroupRepository = Substitute.For<ITaskStatusGroupRepository>();
            statusGroupRepository.AddAsync(Arg.Any<TaskStatusGroup>()).Returns(StatusGroupId)
                .AndDoes(info => statusGroups.Add(info.Arg<TaskStatusGroup>()));

            var statusGroupFactory = Substitute.For<ITaskStatusGroupFactory>();
            statusGroupFactory.Create(Arg.Any<AddTaskStatusGroupRequest>(), true).Returns(info => new TaskStatusGroup
            {
                IsDefault = info.Arg<bool>(),
                Name = info.Arg<AddTaskStatusGroupRequest>().Name,
                UserId = info.Arg<AddTaskStatusGroupRequest>().UserId
            });

            var statusRepository = Substitute.For<IUserTaskStatusRepository>();
            statusRepository.AddAsync(Arg.Any<UserTaskStatus[]>()).Returns(Task.CompletedTask)
                .AndDoes(info => statuses.AddRange(info.Arg<UserTaskStatus[]>()));

            var statusFactory = Substitute.For<IUserTaskStatusFactory>();
            statusFactory.Create(Arg.Any<AddUserTaskStatusRequest>(), true).Returns(info => new UserTaskStatus
            {
                GroupId = info.Arg<AddUserTaskStatusRequest>().GroupId,
                Name = info.Arg<AddUserTaskStatusRequest>().Name
            });

            var validator = Substitute.For<IValidator<RegisterCommand>>();

            validator.ValidateAsync(Arg.Any<RegisterCommand>()).Returns(Task.FromResult(new ValidationResult()));

            var handler = new RegisterCommandHandler(userRepository, hashService, userFactory, validator,
                statusFactory, statusRepository, statusGroupFactory, statusGroupRepository);

            var command = new RegisterCommand
            {
                Email = "email"
            };

            var response = await handler.Handle(command, default);

            Assert.True(response.IsSuccess);
            Assert.Equal(response.NewEntityId, NewId);
            Assert.Contains(users, x => x.Id == NewId && x.Email == command.Email && x.PasswordHash == PasswordHash);
            Assert.Contains(statuses, stat => stat.Name == "Closed");
            Assert.Contains(statuses, stat => stat.Name == "Backlog");
            Assert.All(statuses, stat => Assert.Equal(StatusGroupId, stat.GroupId));
            Assert.Contains(statusGroups, group => group.Name == "Default" && group.UserId == NewId);
        }

        [Fact]
        public async Task RegisterCommand_EmailTaken_Failure()
        {
            var userFactory = Substitute.For<IUserFactory>();

            var hashService = Substitute.For<IHashService>();

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).Returns(new User());

            var statusGroupRepository = Substitute.For<ITaskStatusGroupRepository>();
            var statusGroupFactory = Substitute.For<ITaskStatusGroupFactory>();

            var statusRepository = Substitute.For<IUserTaskStatusRepository>();
            var statusFactory = Substitute.For<IUserTaskStatusFactory>();

            var validator = Substitute.For<IValidator<RegisterCommand>>();

            var handler = new RegisterCommandHandler(userRepository, hashService, userFactory, validator,
                statusFactory, statusRepository, statusGroupFactory, statusGroupRepository);

            var command = new RegisterCommand
            {
                Email = "email"
            };

            var response = await handler.Handle(command, default);

            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.Error);
        }

        [Fact]
        public async Task RegisterCommand_InvalidRequest_Failure()
        {
            var userFactory = Substitute.For<IUserFactory>();

            var hashService = Substitute.For<IHashService>();

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).ReturnsNull();

            var validator = Substitute.For<IValidator<RegisterCommand>>();

            var statusGroupRepository = Substitute.For<ITaskStatusGroupRepository>();
            var statusGroupFactory = Substitute.For<ITaskStatusGroupFactory>();

            var statusRepository = Substitute.For<IUserTaskStatusRepository>();
            var statusFactory = Substitute.For<IUserTaskStatusFactory>();

            validator.ValidateAsync(Arg.Any<RegisterCommand>()).Returns(
                new ValidationResult(new List<ValidationFailure> { new ValidationFailure("prop", "invalid prop") }));

            var handler = new RegisterCommandHandler(userRepository, hashService, userFactory, validator,
                statusFactory, statusRepository, statusGroupFactory, statusGroupRepository);

            var command = new RegisterCommand
            {
                Email = "email"
            };

            var response = await handler.Handle(command, default);

            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.Error);
            Assert.NotEmpty(response.ValidationErrors);
        }
    }
}
