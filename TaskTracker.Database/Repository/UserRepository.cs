using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.User;

namespace TaskTracker.Database.Repository
{
    public interface IUserRepository : IRepositoryBase<User, UserModel>
    {
        Task<User> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
    }

    public class UserRepository : RepositoryBase<User, UserModel>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = UserExpressions.Model;
        }

        public Task<User> GetByEmailAsync(string email)
            => _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == email);

        public Task UpdateAsync(User user)
        {
            _dbContext.Set<User>().Update(user);

            return _dbContext.SaveChangesAsync();
        }
    }
}
