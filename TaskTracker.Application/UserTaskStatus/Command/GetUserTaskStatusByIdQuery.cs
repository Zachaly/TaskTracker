using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskStatusByIdQuery : GetEntityByIdQuery<UserTaskStatusModel>
    {
    }

    public class GetUserTaskStatusByIdHandler : GetEntityByIdHandler<UserTaskStatus, UserTaskStatusModel,
        GetUserTaskStatusRequest, GetUserTaskStatusByIdQuery>
    {
        public GetUserTaskStatusByIdHandler(IUserTaskStatusRepository userTaskStatusRepository)
            : base(userTaskStatusRepository)
        {
        }
    }
}
