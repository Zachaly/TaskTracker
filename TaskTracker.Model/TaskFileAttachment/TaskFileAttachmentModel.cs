namespace TaskTracker.Model.TaskFileAttachment
{
    public class TaskFileAttachmentModel : IModel
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
    }
}
