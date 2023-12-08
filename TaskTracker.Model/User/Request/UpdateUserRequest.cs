namespace TaskTracker.Model.User.Request
{
    public class UpdateUserRequest
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
