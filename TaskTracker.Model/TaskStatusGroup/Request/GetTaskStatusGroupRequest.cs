namespace TaskTracker.Model.TaskStatusGroup.Request
{
    public class GetTaskStatusGroupRequest : PagedRequest
    {
        public long? UserId { get; set; }
    }
}
