using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Database.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(long userId);
        Task AddAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
    }

    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);

            return _dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<RefreshToken>> GetByUserIdAsync(long userId)
            => Task.FromResult(_dbContext.Set<RefreshToken>()
                .Where(t => t.UserId == userId && t.IsValid && t.ExpiryDate >= DateTime.UtcNow)
                .AsEnumerable());

        public Task<RefreshToken?> GetTokenAsync(string token)
            => _dbContext.Set<RefreshToken>().Include(t => t.User).Where(t => 
                t.IsValid 
                && t.Token == token 
                && t.ExpiryDate >= DateTime.UtcNow)
                .SingleOrDefaultAsync();

        public Task UpdateAsync(RefreshToken refreshToken)
        {
            _dbContext.Update(refreshToken);

            return _dbContext.SaveChangesAsync();
        }
    }
}
