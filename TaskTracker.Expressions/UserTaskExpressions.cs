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
                Status = UserTaskStatusExpressions.Model.Compile().Invoke(task.Status),
                Priority = task.Priority,
                AssignedUsers = task.AssignedUsers.Select(u => UserExpressions.Model.Compile().Invoke(u.User)),
                ListId = task.ListId,
                Attachments = task.Attachments.Select(f => TaskFileAttachmentExpressions.Model.Compile().Invoke(f)),
            };
    }
}
