namespace TaskTracker.Model.User.Request
{
    public class ChangeUserPasswordRequest
    {
        public long UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
