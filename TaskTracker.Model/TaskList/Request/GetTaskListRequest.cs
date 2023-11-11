namespace TaskTracker.Model.TaskList.Request
{
    public class GetTaskListRequest : PagedRequest
    {
        public long? CreatorId { get; set; }
    }
}
