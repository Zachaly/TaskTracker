using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskAssignedUser;
using TaskTracker.Model.TaskAssignedUser.Request;

namespace TaskTracker.Application.Command
{
    public class AddTaskAssignedUserCommand : AddTaskAssignedUserRequest, IAddKeylessEntityCommand
    {
    }

    public class AddTaskAssignedUserHandler : AddKeylessEntityHandler<TaskAssignedUser, TaskAssignedUserModel,
        GetTaskAssignedUserRequest, AddTaskAssignedUserRequest, AddTaskAssignedUserCommand>
    {
        public AddTaskAssignedUserHandler(ITaskAssignedUserRepository repository, ITaskAssignedUserFactory entityFactory,
            IValidator<AddTaskAssignedUserCommand> validator) : base(repository, entityFactory, validator)
        {
        }
    }
}
