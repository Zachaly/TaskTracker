using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.User;

namespace TaskTracker.Model.UserSpace
{
    public class UserSpaceModel : IModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public TaskStatusGroupModel StatusGroup { get; set; }
        public UserModel Owner { get; set; }
        public IEnumerable<TaskListModel>? Lists { get; set; }
    }
}
