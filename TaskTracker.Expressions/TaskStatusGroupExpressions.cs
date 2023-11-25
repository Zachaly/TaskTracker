using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup;

namespace TaskTracker.Expressions
{
    public static class TaskStatusGroupExpressions
    {
        public static Expression<Func<TaskStatusGroup, TaskStatusGroupModel>> Model { get; } = group => new TaskStatusGroupModel
        {
            Id = group.Id,
            Name = group.Name,
            Statuses = group.Statuses.AsQueryable().Select(UserTaskStatusExpressions.Model).AsEnumerable(),
            IsDefault = group.IsDefault,
        };
    }
}
