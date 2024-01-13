using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class AddUserTaskCommand : AddUserTaskRequest, IAddEntityCommand
    {
    }

    public class AddUserTaskHandler : AddEntityHandler<UserTask, UserTaskModel, GetUserTaskRequest,
        AddUserTaskRequest, AddUserTaskCommand>
    {
        public AddUserTaskHandler(IUserTaskFactory userTaskFactory, IUserTaskRepository userTaskRepository, 
            IValidator<AddUserTaskCommand> validator) : base(userTaskRepository, userTaskFactory, validator)
        {
        }
    }
}
