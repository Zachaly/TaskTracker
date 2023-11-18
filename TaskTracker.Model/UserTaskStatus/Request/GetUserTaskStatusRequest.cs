namespace TaskTracker.Model.UserTaskStatus.Request
{
    public class GetUserTaskStatusRequest
    {
        public long? GroupId { get; set; }
        public bool? IsDefault { get; set; }
    }
}
