using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus;

namespace TaskTracker.Expressions
{
    public static class UserTaskStatusExpressions
    {
        public static Expression<Func<UserTaskStatus, UserTaskStatusModel>> Model { get; } = status => new UserTaskStatusModel
            {
                Color = status.Color,
                Id = status.Id,
                Index = status.Index,
                Name = status.Name,
            };
    }
}
