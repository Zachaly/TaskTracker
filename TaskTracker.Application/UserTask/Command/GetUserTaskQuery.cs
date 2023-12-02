using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskQuery : GetUserTaskRequest, IGetEntityQuery<UserTaskModel>
    {
    }

    public class GetUserTaskHandler : GetEntityHandler<UserTask, UserTaskModel, GetUserTaskRequest, GetUserTaskQuery>
    {
        public GetUserTaskHandler(IUserTaskRepository userTaskRepository) : base(userTaskRepository)
        {
        }
    }
}
