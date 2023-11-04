namespace TaskTracker.Model.UserTask.Request
{
    public class AddUserTaskRequest
    {
        public long CreatorId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public long? DueTimestamp { get; set; }
        public long? ListId { get; set; }
    }
}
