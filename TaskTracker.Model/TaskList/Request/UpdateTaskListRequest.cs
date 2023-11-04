namespace TaskTracker.Model.TaskList.Request
{
    public class UpdateTaskListRequest
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
