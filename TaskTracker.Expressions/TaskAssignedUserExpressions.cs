using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskAssignedUser;

namespace TaskTracker.Expressions
{
    public static class TaskAssignedUserExpressions
    {
        public static Expression<Func<TaskAssignedUser, TaskAssignedUserModel>> Model { get; } = user => new TaskAssignedUserModel
        {
            Task = UserTaskExpressions.Model.Compile().Invoke(user.Task),
            User = UserExpressions.Model.Compile().Invoke(user.User),
        };
    }
}
