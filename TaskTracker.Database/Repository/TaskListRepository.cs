using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Database.Repository
{
    public interface ITaskListRepository : IRepositoryBase<TaskList, TaskListModel, GetTaskListRequest>
    {
    }

    public class TaskListRepository : RepositoryBase<TaskList, TaskListModel, GetTaskListRequest>, ITaskListRepository
    {
        public TaskListRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = TaskListExpressions.Model;
        }

        public override Task<TaskListModel?> GetByIdAsync(long id)
        {
            var query = FilterById(_dbContext.Set<TaskList>(), id);

            query = query
                .Include(l => l.Creator)
                .Include(l => l.TaskStatusGroup).ThenInclude(g => g.Statuses);

            return query.Select(ModelExpression).FirstOrDefaultAsync();
        }

        public override Task<IEnumerable<TaskListModel>> GetAsync(GetTaskListRequest request)
        {
            var query = FilterWithRequest(_dbContext.Set<TaskList>(), request);

            query = query.Include(l => l.Creator);

            if (request.JoinTasks.GetValueOrDefault())
            {
                query = query.Include(l => l.Tasks);
            }

            if (request.JoinStatusGroup.GetValueOrDefault())
            {
                query = query.Include(l => l.TaskStatusGroup).ThenInclude(g => g.Statuses);
            }

            return Task.FromResult(AddPagination(query, request).Select(ModelExpression).AsEnumerable());
        }

        public override Task DeleteByIdAsync(long id)
        {
            var tasks = _dbContext.Set<UserTask>().Where(t => t.ListId == id);
            _dbContext.Set<UserTask>().RemoveRange(tasks);

            return base.DeleteByIdAsync(id);
        }
    }
}
