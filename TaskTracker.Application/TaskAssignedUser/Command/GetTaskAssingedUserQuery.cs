using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskAssignedUser;
using TaskTracker.Model.TaskAssignedUser.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskAssignedUserQuery : GetTaskAssignedUserRequest, IGetEntityQuery<TaskAssignedUserModel>
    {
    }

    public class GetTaskAssignedUserHandler : GetEntityHandler<TaskAssignedUser, TaskAssignedUserModel, GetTaskAssignedUserRequest,
        GetTaskAssignedUserQuery>
    {
        public GetTaskAssignedUserHandler(ITaskAssignedUserRepository repository) : base(repository)
        {
        }
    }
}
