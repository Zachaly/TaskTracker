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

        public override Task<IEnumerable<TaskAssignedUserModel>> GetAsync(GetTaskAssignedUserRequest request)
        {
            var query = FilterWithRequest(_dbContext.Set<TaskAssignedUser>(), request);

            query = OrderBy(query, request);

            query = query
                .Include(u => u.Task)
                .ThenInclude(t => t.AssignedUsers)
                .ThenInclude(u => u.User)
                .Include(u => u.Task)
                .ThenInclude(t => t.Status)
                .Include(u => u.Task)
                .ThenInclude(t => t.Creator)
                .Include(u => u.Task)
                .ThenInclude(u => u.Attachments);

            return Task.FromResult(AddPagination(query, request).Select(ModelExpression).AsEnumerable());
        }

        public async Task DeleteByTaskIdAndUserIdAsync(long taskId, long userId)
        {
            var user = await _dbContext.Set<TaskAssignedUser>().FirstOrDefaultAsync(u => u.TaskId == taskId && u.UserId == userId);

            if(user is null)
            {
                throw new EntityNotFoundException(nameof(TaskAssignedUser));
            }

            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();
        }
    }
}
