using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Database.Repository
{
    public interface ITaskStatusGroupRepository : IRepositoryBase<TaskStatusGroup, TaskStatusGroupModel>
    {
        Task UpdateAsync(TaskStatusGroup entity);
        Task<IEnumerable<TaskStatusGroupModel>> GetAsync(GetTaskStatusGroupRequest request);
    }

    public class TaskStatusGroupRepository : RepositoryBase<TaskStatusGroup, TaskStatusGroupModel>, ITaskStatusGroupRepository
    {
        public TaskStatusGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<TaskStatusGroupModel>> GetAsync(GetTaskStatusGroupRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TaskStatusGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
