using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.User;
using TaskTracker.Model.UserTask;

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
        public IEnumerable<UserTaskModel> Tasks { get; set; }
        public TaskStatusGroupModel? StatusGroup { get; set; }
    }
}
