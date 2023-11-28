namespace TaskTracker.Domain.Entity
{
    public class UserTaskStatus : IEntity
    {
        public long Id { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public long GroupId { get; set; }
        public int Index { get; set; }
        public TaskStatusGroup Group { get; set; }
        public ICollection<UserTask> Tasks { get; set; }
    }
}
