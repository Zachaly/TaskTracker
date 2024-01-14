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

        public override Task<IEnumerable<UserTaskModel>> GetAsync(GetUserTaskRequest request)
        {
            var query = FilterWithRequest(_dbContext.Set<UserTask>(), request);

            query = OrderBy(query, request);

            query = query.Include(t => t.Creator)
                .Include(t => t.AssignedUsers)
                .ThenInclude(u => u.User);

            return Task.FromResult(AddPagination(query, request).Select(ModelExpression).AsEnumerable());
        }

        public override Task<UserTaskModel?> GetByIdAsync(long id)
        {
            var queryable = _dbContext.Set<UserTask>()
                .Include(t => t.Creator)
                .Include(t => t.AssignedUsers)
                .ThenInclude(u => u.User);
            
            return FilterById(queryable, id).Select(ModelExpression).FirstOrDefaultAsync();
        }
    }
}
