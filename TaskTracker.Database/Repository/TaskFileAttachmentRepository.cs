using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.TaskFileAttachment;
using TaskTracker.Model.TaskFileAttachment.Request;

namespace TaskTracker.Database.Repository
{
    public interface ITaskFileAttachmentRepository : IRepositoryBase<TaskFileAttachment, TaskFileAttachmentModel, GetTaskFileAttachmentRequest>
    {
        
    }

    public class TaskFileAttachmentRepository : RepositoryBase<TaskFileAttachment, TaskFileAttachmentModel, GetTaskFileAttachmentRequest>,
        ITaskFileAttachmentRepository
    {
        public TaskFileAttachmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = TaskFileAttachmentExpressions.Model;
        }
    }
}
