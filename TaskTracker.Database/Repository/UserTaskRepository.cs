using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Database.Repository
{
    public interface IUserTaskRepository : IRepositoryBase<UserTask, UserTaskModel>
    {
        Task<IEnumerable<UserTaskModel>> GetAsync(GetUserTaskRequest request);
    }

    public class UserTaskRepository : RepositoryBase<UserTask, UserTaskModel>, IUserTaskRepository
    {
        public UserTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = UserTaskExpressions.Model;
        }

        public Task<IEnumerable<UserTaskModel>> GetAsync(GetUserTaskRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
