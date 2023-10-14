using TaskTracker.Domain.Entity;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Application
{
    public interface IUserFactory
    {
        User Create(RegisterRequest request, string passwordHash);
    }

    public class UserFactory : IUserFactory
    {
        public User Create(RegisterRequest request, string passwordHash)
            => new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
    }
}
