using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class SpaceUserPermissionsCommandTests
    {
        [Fact]
        public async Task UpdateSpaceUserPermissionsCommand_ValidRequest_Success()
        {
            var command = new UpdateSpaceUserPermissionsCommand
            {
                UserId = 1,
                SpaceId = 2,
                CanAddUsers = true,
                CanAssignTaskUsers = true,
                CanChangePermissions = true,
                CanModifyLists = true,
                CanModifySpace = true,
                CanModifyStatusGroups = true,
                CanModifyTasks = true,
                CanRemoveLists = true,
                CanRemoveTasks = true,
                CanRemoveUsers = true,
            };

            var permissions = new SpaceUserPermissions();

            var repository = Substitute.For<ISpaceUserPermissionsRepository>();

            repository.GetBySpaceIdAndUserIdAsync(command.SpaceId, command.UserId).Returns(permissions);

            repository.UpdateAsync(permissions).Returns(Task.CompletedTask);

            var validator = Substitute.For<IValidator<UpdateSpaceUserPermissionsCommand>>();

            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateSpaceUserPermissionsHandler(repository, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(permissions);

            Assert.True(res.IsSuccess);
            Assert.Equal(permissions.CanAddUsers, permissions.CanAddUsers);
            Assert.Equal(permissions.CanAssignTaskUsers, permissions.CanAssignTaskUsers);
            Assert.Equal(permissions.CanChangePermissions, permissions.CanChangePermissions);
            Assert.Equal(permissions.CanModifyLists, permissions.CanModifyLists);
            Assert.Equal(permissions.CanModifySpace, permissions.CanModifySpace);
            Assert.Equal(permissions.CanModifyStatusGroups, permissions.CanModifyStatusGroups);
            Assert.Equal(permissions.CanModifyTasks, permissions.CanModifyTasks);
            Assert.Equal(permissions.CanRemoveLists, permissions.CanRemoveLists);
            Assert.Equal(permissions.CanRemoveTasks, permissions.CanRemoveTasks);
            Assert.Equal(permissions.CanRemoveUsers, permissions.CanRemoveUsers);
        }

        [Fact]
        public async Task UpdateSpaceUserPermissionsCommand_InvalidRequest_Failure()
        {
            var command = new UpdateSpaceUserPermissionsCommand();

            var repository = Substitute.For<ISpaceUserPermissionsRepository>();

            var validator = Substitute.For<IValidator<UpdateSpaceUserPermissionsCommand>>();

            validator.Validate(command).Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("prop", "err")
            }));

            var handler = new UpdateSpaceUserPermissionsHandler(repository, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.ValidationErrors);
        }

        [Fact]
        public async Task UpdateSpaceUserPermissionsCommand_EntityNotFound_Failure()
        {
            var command = new UpdateSpaceUserPermissionsCommand
            {
                UserId = 1,
                SpaceId = 2,
            };

            var repository = Substitute.For<ISpaceUserPermissionsRepository>();

            repository.GetBySpaceIdAndUserIdAsync(command.SpaceId, command.UserId).ReturnsNull();

            var validator = Substitute.For<IValidator<UpdateSpaceUserPermissionsCommand>>();

            validator.Validate(command).Returns(new ValidationResult());

            var handler = new UpdateSpaceUserPermissionsHandler(repository, validator);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotEmpty(res.Error);
        }
    }
}
