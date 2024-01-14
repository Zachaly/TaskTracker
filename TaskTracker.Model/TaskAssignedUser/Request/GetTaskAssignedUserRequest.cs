namespace TaskTracker.Model.TaskAssignedUser.Request
{
    public class GetTaskAssignedUserRequest : PagedRequest
    {
        public long? TaskId { get; set; }
        public long? UserId { get; set; }
    }
}
