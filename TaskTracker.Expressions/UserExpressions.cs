using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.User;

namespace TaskTracker.Expressions
{
    public static class UserExpressions
    {
        public static Expression<Func<User, UserModel>> Model { get; } = user =>
            new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
    }
}
