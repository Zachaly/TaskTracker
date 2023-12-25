namespace TaskTracker.Domain.Entity
{
    public class TaskTrackerDocumentPage : IEntity
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
        public long DocumentId { get; set; }
        public TaskTrackerDocument Document { get; set; }
        public long LastModifiedTimestamp { get; set; }
    }
}
