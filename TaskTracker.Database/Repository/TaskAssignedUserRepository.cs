using Microsoft.EntityFrameworkCore;
using TaskTracker.Database.Exception;
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

        public async Task DeleteByTaskIdAndUserIdAsync(long taskId, long userId)
        {
            var user = await _dbContext.Set<TaskAssignedUser>().FirstAsync(u => u.TaskId == taskId && u.UserId == userId);

            if(user is null)
            {
                throw new EntityNotFoundException(nameof(TaskAssignedUser));
            }

            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();
        }
    }
}
