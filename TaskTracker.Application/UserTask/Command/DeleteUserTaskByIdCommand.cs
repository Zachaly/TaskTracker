using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class DeleteUserTaskByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteUserTaskByIdHandler : DeleteEntityByIdHandler<UserTask, UserTaskModel,
        GetUserTaskRequest, DeleteUserTaskByIdCommand>
    { 
        public DeleteUserTaskByIdHandler(IUserTaskRepository userTaskRepository) : base(userTaskRepository)
        {
        }
    }
}
