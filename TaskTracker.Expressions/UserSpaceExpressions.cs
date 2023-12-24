using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;

namespace TaskTracker.Expressions
{
    public static class UserSpaceExpressions
    {
        public static Expression<Func<UserSpace, UserSpaceModel>> Model { get; } = space => new UserSpaceModel()
            {
                Id = space.Id,
                Lists = space.Lists.AsQueryable().Select(TaskListExpressions.Model).AsEnumerable(),
                Owner = UserExpressions.Model.Compile().Invoke(space.Owner),
                StatusGroup = TaskStatusGroupExpressions.Model.Compile().Invoke(space.StatusGroup),
                Title = space.Title,
            };
    }
}
