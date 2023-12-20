using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;

namespace TaskTracker.Expressions
{
    public class UserSpaceExpressions
    {
        public static Expression<Func<UserSpace, UserSpaceModel>> Model { get; set; } = space => new UserSpaceModel
            {
                Id = space.Id,
                Lists = space.Lists != null ? space.Lists.AsQueryable().Select(TaskListExpressions.Model).AsEnumerable() : null,
                Owner = UserExpressions.Model.Compile().Invoke(space.Owner),
                StatusGroup = TaskStatusGroupExpressions.Model.Compile().Invoke(space.StatusGroup),
                Title = space.Title,
            };
    }
}
