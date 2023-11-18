using TaskTracker.Model.User;

namespace TaskTracker.Model.TaskList
{
    public class TaskListModel : IModel
    {
        public long Id { get; set; }
        public UserModel Creator { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public long StatusGroupId { get; set; }
    }
}
