namespace TaskTracker.Model.UserTaskStatus
{
    public class UserTaskStatusModel : IModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Index { get; set; }
        public bool IsDefault { get; set; }
    }
}
