using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Database.Repository
{
    public interface IUserTaskRepository : IRepositoryBase<UserTask, UserTaskModel, GetUserTaskRequest>
    {
    }

    public class UserTaskRepository : RepositoryBase<UserTask, UserTaskModel, GetUserTaskRequest>,
        IUserTaskRepository
    {
        public UserTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = UserTaskExpressions.Model;
        }

        public override Task<UserTaskModel?> GetByIdAsync(long id)
        {
            var queryable = _dbContext.Set<UserTask>()
                .Include(t => t.Creator);
            
            return FilterById(queryable, id).Select(ModelExpression).FirstOrDefaultAsync();
        }
    }
}
