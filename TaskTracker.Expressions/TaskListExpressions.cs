using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;

namespace TaskTracker.Expressions
{
    public static class TaskListExpressions
    {
        public static Expression<Func<TaskList, TaskListModel>> Model => list => new TaskListModel
        {
            Color = list.Color,
            Creator = UserExpressions.Model.Compile().Invoke(list.Creator),
            Description = list.Description,
            Id = list.Id,
            Title = list.Title,
            StatusGroupId = list.TaskStatusGroupId,
            Tasks = list.Tasks.AsQueryable().Select(UserTaskExpressions.Model).AsEnumerable(),
            StatusGroup = list.TaskStatusGroup == null ? TaskStatusGroupExpressions.Model.Compile().Invoke(list.TaskStatusGroup) : null
        };
    }
}
