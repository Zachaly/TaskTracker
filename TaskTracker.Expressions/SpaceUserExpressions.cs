using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUser;

namespace TaskTracker.Expressions
{
    public static class SpaceUserExpressions
    {
        public static Expression<Func<SpaceUser, SpaceUserModel>> Model { get; } = user => new SpaceUserModel
        {
            Email = user.User.Email,
            FirstName = user.User.FirstName,
            Id = user.User.Id,
            LastName = user.User.LastName,
        };
    }
}
