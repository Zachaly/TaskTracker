namespace TaskTracker.Model.TaskList.Request
{
    public class AddTaskListRequest
    {
        public long CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}
