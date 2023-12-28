namespace TaskTracker.Domain.Entity
{
    public class TaskTrackerDocument : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long CreationTimestamp { get; set; }
        public ICollection<TaskTrackerDocumentPage> Pages { get; set; }
        public long SpaceId { get; set; }
        public UserSpace Space { get; set; }
        public long CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
