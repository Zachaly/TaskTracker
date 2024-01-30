namespace TaskTracker.Domain.Entity
{
    public class TaskFileAttachment : IEntity
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public UserTask Task { get; set; }
        public string FileName { get; set; }
    }
}
