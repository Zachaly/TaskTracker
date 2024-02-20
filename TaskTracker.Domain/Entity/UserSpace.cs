namespace TaskTracker.Domain.Entity
{
    public class UserSpace : IEntity
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public User Owner { get; set; }
        public string Title { get; set; }
        public long StatusGroupId { get; set; }
        public TaskStatusGroup StatusGroup { get; set; }
        public ICollection<TaskList> Lists { get; set; }
        public ICollection<TaskTrackerDocument> Documents { get; set; }
        public ICollection<SpaceUser> Users { get; set; }
        public ICollection<SpaceUserPermissions> Permissions { get; set; }
    }
}
