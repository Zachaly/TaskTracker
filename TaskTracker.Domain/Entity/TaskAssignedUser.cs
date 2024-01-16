namespace TaskTracker.Domain.Entity
{
    public class TaskAssignedUser : IKeylessEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long TaskId { get; set; }
        public UserTask Task { get; set; }
    }
}
