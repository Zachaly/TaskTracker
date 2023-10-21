using TaskTracker.Domain.Entity;

namespace TaskTracker.Application
{
    public interface IRefreshTokenFactory
    {
        RefreshToken Create(long userId, string token);
    }

    public class RefreshTokenFactory : IRefreshTokenFactory
    {
        public RefreshToken Create(long userId, string token)
            => new RefreshToken
            {
                UserId = userId,
                Token = token,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                IsValid = true,
            };
    }
}
