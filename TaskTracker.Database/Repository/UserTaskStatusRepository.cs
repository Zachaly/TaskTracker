using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.UserTaskStatus;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Database.Repository
{
    public interface IUserTaskStatusRepository : IRepositoryBase<UserTaskStatus, UserTaskStatusModel, GetUserTaskStatusRequest>
    {
        Task UpdateAsync(UserTaskStatus entity);
    }

    public class UserTaskStatusRepository : RepositoryBase<UserTaskStatus, UserTaskStatusModel, GetUserTaskStatusRequest>,
        IUserTaskStatusRepository
    {
        public UserTaskStatusRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = UserTaskStatusExpressions.Model;
        }

        public Task UpdateAsync(UserTaskStatus entity)
        {
            _dbContext.Set<UserTaskStatus>().Update(entity);

            return _dbContext.SaveChangesAsync();
        }
    }
}
