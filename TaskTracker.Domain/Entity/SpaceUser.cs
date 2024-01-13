namespace TaskTracker.Domain.Entity
{
    public class SpaceUser : IKeylessEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long SpaceId { get; set; }
        public UserSpace Space { get; set; }
    }
}
