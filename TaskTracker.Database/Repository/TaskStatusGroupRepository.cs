using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup;

namespace TaskTracker.Database.Repository
{
    public interface ITaskStatusGroupRepository : IRepositoryBase<TaskStatusGroup, TaskStatusGroupModel>
    {
        Task UpdateAsync(TaskStatusGroup entity);
    }

    public class TaskStatusGroupRepository : RepositoryBase<TaskStatusGroup, TaskStatusGroupModel>, ITaskStatusGroupRepository
    {
        public TaskStatusGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task UpdateAsync(TaskStatusGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
