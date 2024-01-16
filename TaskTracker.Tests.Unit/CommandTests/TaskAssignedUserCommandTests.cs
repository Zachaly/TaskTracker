using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TaskTracker.Application.Command;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class TaskAssignedUserCommandTests
    {
        [Fact]
        public async Task DeleteTaskAssignedUserCommand_DeletesUser()
        {
            var command = new DeleteTaskAssignedUserCommand(1, 2);

            var repository = Substitute.For<ITaskAssignedUserRepository>();

            repository.DeleteByTaskIdAndUserIdAsync(command.TaskId, command.UserId).Returns(Task.CompletedTask);

            var handler = new DeleteTaskAssignedUserHandler(repository);

            var res = await handler.Handle(command, default);

            await repository.Received(1).DeleteByTaskIdAndUserIdAsync(command.TaskId, command.UserId);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteTaskAssignedUserCommand_EntityNotFoundException_Failure()
        {
            var command = new DeleteTaskAssignedUserCommand(1, 2);

            var repository = Substitute.For<ITaskAssignedUserRepository>();

            repository.DeleteByTaskIdAndUserIdAsync(command.TaskId, command.UserId)
                .Throws(new EntityNotFoundException("entity"));

            var handler = new DeleteTaskAssignedUserHandler(repository);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }
    }
}
