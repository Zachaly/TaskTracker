using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskByIdQuery : GetEntityByIdQuery<UserTaskModel>
    {
    }

    public class GetUserTaskByIdHandler : GetEntityByIdHandler<UserTask, UserTaskModel, GetUserTaskByIdQuery>
    {
        public GetUserTaskByIdHandler(IUserTaskRepository userTaskRepository) : base(userTaskRepository)
        {
        }
    }
}
