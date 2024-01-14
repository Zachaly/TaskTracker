using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.TaskAssignedUser;
using TaskTracker.Model.TaskAssignedUser.Request;

namespace TaskTracker.Database.Repository
{
    public interface ITaskAssignedUserRepository : IKeylessRepositoryBase<TaskAssignedUser, TaskAssignedUserModel, GetTaskAssignedUserRequest>
    {
        Task DeleteByTaskIdAndUserIdAsync(long taskId, long userId);
    }

    public class TaskAssignedUserRepository : KeylessRepositoryBase<TaskAssignedUser, TaskAssignedUserModel, GetTaskAssignedUserRequest>,
        ITaskAssignedUserRepository
    {
        public TaskAssignedUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = TaskAssignedUserExpressions.Model;
        }

        public Task DeleteByTaskIdAndUserIdAsync(long taskId, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
