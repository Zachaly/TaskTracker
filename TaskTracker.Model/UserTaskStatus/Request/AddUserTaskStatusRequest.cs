namespace TaskTracker.Model.UserTaskStatus.Request
{
    public class AddUserTaskStatusRequest
    {
        public long GroupId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Index { get; set; }
    }
}
