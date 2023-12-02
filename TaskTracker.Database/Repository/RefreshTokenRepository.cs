using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.RefreshToken;

namespace TaskTracker.Database.Repository
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken, RefreshTokenModel>
    {
        Task<RefreshToken?> GetTokenAsync(string token);
    }

    public class RefreshTokenRepository : RepositoryBase<RefreshToken, RefreshTokenModel>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = RefreshTokenExpressions.Model;
        }

        public Task<RefreshToken?> GetTokenAsync(string token)
            => _dbContext.Set<RefreshToken>().Include(t => t.User).Where(t => 
                t.IsValid 
                && t.Token == token 
                && t.ExpiryDate >= DateTime.UtcNow)
                .SingleOrDefaultAsync();
    }
}
