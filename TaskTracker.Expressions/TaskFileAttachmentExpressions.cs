using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskFileAttachment;

namespace TaskTracker.Expressions
{
    public static class TaskFileAttachmentExpressions
    {
        public static Expression<Func<TaskFileAttachment, TaskFileAttachmentModel>> Model { get; } = file => new TaskFileAttachmentModel
        {
            Id = file.Id,
            FileName = file.FileName,
            Type = file.FileName.Substring(file.FileName.LastIndexOf('.'))
        };
    }
}
