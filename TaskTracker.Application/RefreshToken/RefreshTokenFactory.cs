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
        {
            throw new NotImplementedException();
        }
    }
}
