using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskAssignedUser.Request;

namespace TaskTracker.Application
{
    public interface ITaskAssignedUserFactory : IEntityFactory<TaskAssignedUser, AddTaskAssignedUserRequest>
    {
    }

    public class TaskAssignedUserFactory : ITaskAssignedUserFactory
    {
        public TaskAssignedUser Create(AddTaskAssignedUserRequest request)
            => new TaskAssignedUser
            {
                TaskId = request.TaskId,
                UserId = request.UserId,
            };
    }
}
