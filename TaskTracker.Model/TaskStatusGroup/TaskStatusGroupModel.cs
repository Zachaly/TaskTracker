using TaskTracker.Model.UserTaskStatus;

namespace TaskTracker.Model.TaskStatusGroup
{
    public class TaskStatusGroupModel : IModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserTaskStatusModel> Statuses { get; set; }
    }
}
