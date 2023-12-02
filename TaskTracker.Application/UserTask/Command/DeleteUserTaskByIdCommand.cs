using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Application.Command
{
    public class DeleteUserTaskByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteUserTaskByIdHandler : DeleteEntityByIdHandler<UserTask, UserTaskModel, DeleteUserTaskByIdCommand>
    { 
        public DeleteUserTaskByIdHandler(IUserTaskRepository userTaskRepository) : base(userTaskRepository)
        {
        }
    }
}
