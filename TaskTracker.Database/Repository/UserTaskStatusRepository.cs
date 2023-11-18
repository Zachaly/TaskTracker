
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Database.Repository
{
    public interface IUserTaskStatusRepository : IRepositoryBase<UserTaskStatus, UserTaskStatusModel>
    {
        Task<IEnumerable<UserTaskStatusModel>> GetAsync(GetUserTaskStatusRequest request);
        Task UpdateAsync(UserTaskStatus entity);
    }

    public class UserTaskStatusRepository : RepositoryBase<UserTaskStatus, UserTaskStatusModel>, IUserTaskStatusRepository
    {
        public UserTaskStatusRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<UserTaskStatusModel>> GetAsync(GetUserTaskStatusRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserTaskStatus entity)
        {
            throw new NotImplementedException();
        }
    }
}
