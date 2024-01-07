using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TaskTracker.Application;
using TaskTracker.Application.Command;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUser;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class SpaceUserCommandTests
    {
        [Fact]
        public async Task GetSpaceUserQuery_ReturnsSpaceUsers()
        {
            var query = new GetSpaceUserQuery();

            var repository = Substitute.For<ISpaceUserRepository>();

            var users = new List<SpaceUserModel>();

            repository.GetAsync(query).Returns(users);

            var handler = new GetSpaceUserHandler(repository);

            var res = await handler.Handle(query, default);

            Assert.Equal(users, res);
        }

        [Fact]
        public async Task AddSpaceUserCommand_AddsSpaceUser()
        {
            var command = new AddSpaceUserCommand
            {
                UserId = 1,
                SpaceId = 2
            };

            var users = new List<SpaceUser>();

            var repository = Substitute.For<ISpaceUserRepository>();

            repository.AddAsync(Arg.Any<SpaceUser>()).Returns(Task.CompletedTask)
                .AndDoes(info => users.Add(info.Arg<SpaceUser>()));

            var factory = Substitute.For<ISpaceUserFactory>();

            factory.Create(command).Returns(info => new SpaceUser
            {
                UserId = info.Arg<AddSpaceUserRequest>().UserId,
            });

            var validator = Substitute.For<IValidator<AddSpaceUserCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new AddSpaceUserHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
            Assert.Contains(users, u => u.UserId == command.UserId);
        }

        [Fact]
        public async Task AddSpaceUserCommand_InvalidRequest_Failure()
        {
            var command = new AddSpaceUserCommand
            {
                UserId = 1,
                SpaceId = 2
            };

            var repository = Substitute.For<ISpaceUserRepository>();

            var factory = Substitute.For<ISpaceUserFactory>();

            var validator = Substitute.For<IValidator<AddSpaceUserCommand>>();
            validator.ValidateAsync(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new AddSpaceUserHandler(repository, factory, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task DeleteSpaceUserCommand_DeletesUser()
        {
            var command = new DeleteSpaceUserCommand(1, 2);

            var repository = Substitute.For<ISpaceUserRepository>();

            repository.DeleteByUserIdAndSpaceIdAsync(command.UserId, command.SpaceId).Returns(Task.CompletedTask);

            var handler = new DeleteSpaceUserHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(1).DeleteByUserIdAndSpaceIdAsync(command.UserId, command.SpaceId);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteSpaceUserCommand_EntityNotFoundException_Failure()
        {
            var command = new DeleteSpaceUserCommand(1, 2);

            var repository = Substitute.For<ISpaceUserRepository>();

            repository.DeleteByUserIdAndSpaceIdAsync(command.UserId, command.SpaceId)
                .ThrowsAsync(new EntityNotFoundException("err"));

            var handler = new DeleteSpaceUserHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }
    }
}
