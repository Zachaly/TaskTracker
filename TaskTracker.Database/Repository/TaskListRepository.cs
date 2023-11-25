using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Database.Repository
{
    public interface ITaskListRepository : IRepositoryBase<TaskList, TaskListModel, GetTaskListRequest>
    {
        Task UpdateAsync(TaskList list);
    }

    public class TaskListRepository : RepositoryBase<TaskList, TaskListModel, GetTaskListRequest>, ITaskListRepository
    {
        public TaskListRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = TaskListExpressions.Model;
        }

        public override Task<IEnumerable<TaskListModel>> GetAsync(GetTaskListRequest request)
        {
            var query = FilterWithRequest(_dbContext.Set<TaskList>(), request);

            query = query.Include(l => l.Creator);

            return Task.FromResult(AddPagination(query, request).Select(ModelExpression).AsEnumerable());
        }

        public Task UpdateAsync(TaskList list)
        {
            _dbContext.Set<TaskList>().Update(list);

            return _dbContext.SaveChangesAsync();
        }

        public override Task DeleteByIdAsync(long id)
        {
            var tasks = _dbContext.Set<UserTask>().Where(t => t.ListId == id);
            _dbContext.Set<UserTask>().RemoveRange(tasks);

            return base.DeleteByIdAsync(id);
        }
    }
}
