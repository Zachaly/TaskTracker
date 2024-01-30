namespace TaskTracker.Model.TaskFileAttachment.Request
{
    public class GetTaskFileAttachmentRequest : PagedRequest
    {
        public long? TaskId { get; set; }
    }
}
