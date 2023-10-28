using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Expressions
{
    public static class UserTaskExpressions
    {
        public static Expression<Func<UserTask, UserTaskModel>> Model { get; } = task => new UserTaskModel
            {
                Id = task.Id,
                CreationTimestamp = task.CreationTimestamp,
                Creator = UserExpressions.Model.Compile().Invoke(task.Creator),
                Description = task.Description,
                DueTimestamp = task.DueTimestamp,
                Title = task.Title,
            };
    }
}
