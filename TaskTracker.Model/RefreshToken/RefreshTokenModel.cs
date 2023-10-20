namespace TaskTracker.Model.RefreshToken
{
    public class RefreshTokenModel : IModel
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
