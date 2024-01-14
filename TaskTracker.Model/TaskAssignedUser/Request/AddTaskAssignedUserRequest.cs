namespace TaskTracker.Model.TaskAssignedUser.Request
{
    public class AddTaskAssignedUserRequest
    {
        public long UserId { get; set; }
        public long TaskId { get; set; }
    }
}
