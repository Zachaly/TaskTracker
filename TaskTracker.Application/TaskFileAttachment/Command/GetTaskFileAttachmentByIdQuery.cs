using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskFileAttachment;
using TaskTracker.Model.TaskFileAttachment.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskFileAttachmentByIdQuery : GetEntityByIdQuery<TaskFileAttachmentModel>
    {
    }

    public class GetTaskFileAttachmentByIdHandler : GetEntityByIdHandler<TaskFileAttachment, TaskFileAttachmentModel,
        GetTaskFileAttachmentRequest, GetTaskFileAttachmentByIdQuery>
    {
        public GetTaskFileAttachmentByIdHandler(ITaskFileAttachmentRepository repository) : base(repository)
        {
        }
    }
}
