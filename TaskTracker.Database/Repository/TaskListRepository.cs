using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Database.Repository
{
    public interface ITaskListRepository : IRepositoryBase<TaskList, TaskListModel>
    {
        Task<IEnumerable<TaskListModel>> GetAsync(GetTaskListRequest request);
        Task UpdateAsync(TaskList list);
    }

    public class TaskListRepository : RepositoryBase<TaskList, TaskListModel>, ITaskListRepository
    {
        public TaskListRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = TaskListExpressions.Model;
        }

        public Task<IEnumerable<TaskListModel>> GetAsync(GetTaskListRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TaskList list)
        {
            throw new NotImplementedException();
        }
    }
}
