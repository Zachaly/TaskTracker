namespace TaskTracker.Domain.Entity
{
    public class TaskList : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public ICollection<UserTask> Tasks { get; set; }
        public long CreatorId { get; set; }
        public User Creator { get; set; }
        public long TaskStatusGroupId { get; set; }
        public TaskStatusGroup TaskStatusGroup { get; set; }
        public long SpaceId { get; set; }
        public UserSpace Space { get; set; }
    }
}
