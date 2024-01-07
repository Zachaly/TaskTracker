using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskByIdQuery : GetEntityByIdQuery<UserTaskModel>
    {
    }

    public class GetUserTaskByIdHandler : GetEntityByIdHandler<UserTask, UserTaskModel, GetUserTaskRequest, GetUserTaskByIdQuery>
    {
        public GetUserTaskByIdHandler(IUserTaskRepository userTaskRepository) : base(userTaskRepository)
        {
        }
    }
}
