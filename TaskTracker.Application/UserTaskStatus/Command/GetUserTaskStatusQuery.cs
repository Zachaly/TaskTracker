using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskStatusQuery : GetUserTaskStatusRequest, IGetEntityQuery<UserTaskStatusModel>
    {
    }

    public class GetUserTaskStatusHandler : GetEntityHandler<UserTaskStatus, UserTaskStatusModel,
        GetUserTaskStatusRequest, GetUserTaskStatusQuery>
    {
        public GetUserTaskStatusHandler(IUserTaskStatusRepository userTaskStatusRepository) : base(userTaskStatusRepository)
        {
        }
    }
}
