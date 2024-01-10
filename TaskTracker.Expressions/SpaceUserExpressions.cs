using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUser;
using TaskTracker.Model.UserSpace;

namespace TaskTracker.Expressions
{
    public static class SpaceUserExpressions
    {
        public static Expression<Func<SpaceUser, SpaceUserModel>> Model { get; } = user => new SpaceUserModel
        {
            User = user.User == null ? null : UserExpressions.Model.Compile().Invoke(user.User),
            Space = user.Space == null ? null : new UserSpaceModel
            {
                Id = user.Space.Id,
                Title = user.Space.Title
            }
        };
    }
}
