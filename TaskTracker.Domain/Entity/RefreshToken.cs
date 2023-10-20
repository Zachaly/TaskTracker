namespace TaskTracker.Domain.Entity
{
    public class RefreshToken : IEntity
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsValid { get; set; }
    }
}
