using TaskTracker.Model.User;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Model.TaskAssignedUser
{
    public class TaskAssignedUserModel : IModel
    {
        public UserTaskModel Task { get; set; }
        public UserModel User { get; set; }
    }
}
