using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.User;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Database.Repository
{
    public interface IUserRepository : IRepositoryBase<User, UserModel, GetUserRequest>
    {
        Task<User> GetByEmailAsync(string email);
    }

    public class UserRepository : RepositoryBase<User, UserModel, GetUserRequest>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = UserExpressions.Model;
        }

        public Task<User> GetByEmailAsync(string email)
            => _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }
}
