namespace TaskTracker.Domain.Entity
{
    public class User : IEntity
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? ProfilePicture { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<UserTask> Tasks { get; set; }
        public ICollection<TaskList> Lists { get; set; }
        public ICollection<TaskStatusGroup> StatusGroups { get; set; }
        public ICollection<UserSpace> Spaces { get; set; }
        public ICollection<TaskTrackerDocument> Documents { get; set; }
        public ICollection<SpaceUser> SpaceUsers { get; set; }
    }
}
