using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskFileAttachment;
using TaskTracker.Model.TaskFileAttachment.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskFileAttachmentQuery : GetTaskFileAttachmentRequest, IGetEntityQuery<TaskFileAttachmentModel>
    {
    }

    public class GetTaskFileAttachmentHandler : GetEntityHandler<TaskFileAttachment, TaskFileAttachmentModel,
        GetTaskFileAttachmentRequest, GetTaskFileAttachmentQuery>
    {
        public GetTaskFileAttachmentHandler(ITaskFileAttachmentRepository repository) : base(repository)
        {
        }
    }
}
