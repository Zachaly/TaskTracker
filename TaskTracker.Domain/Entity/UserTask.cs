using TaskTracker.Domain.Enum;

namespace TaskTracker.Domain.Entity
{
    public class UserTask : IEntity
    {
        public long Id { get; set; }
        public long CreatorId { get; set; }
        public User Creator { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long CreationTimestamp { get; set; }
        public long? DueTimestamp { get; set; }
        public long ListId { get; set; }
        public TaskList List { get; set; }
        public long StatusId { get; set; }
        public UserTaskStatus Status { get; set; }
        public UserTaskPriority? Priority { get; set; }
        public ICollection<TaskAssignedUser> AssignedUsers { get; set; }
    }
}
