namespace TaskTracker.Model.UserTaskStatus.Request
{
    public class UpdateUserTaskStatusRequest
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public int? Index { get; set; }
    }
}
