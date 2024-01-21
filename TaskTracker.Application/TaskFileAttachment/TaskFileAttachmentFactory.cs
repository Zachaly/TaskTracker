using TaskTracker.Domain.Entity;

namespace TaskTracker.Application
{
    public interface ITaskFileAttachmentFactory
    {
        TaskFileAttachment Create(long taskId, string fileName);
    }

    public class TaskFileAttachmentFactory : ITaskFileAttachmentFactory
    {
        public TaskFileAttachment Create(long taskId, string fileName)
            => new TaskFileAttachment
            {
                TaskId = taskId,
                FileName = fileName
            };
    }
}
