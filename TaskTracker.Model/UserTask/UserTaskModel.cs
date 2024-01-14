using TaskTracker.Domain.Enum;
using TaskTracker.Model.User;
using TaskTracker.Model.UserTaskStatus;

namespace TaskTracker.Model.UserTask
{
    public class UserTaskModel : IModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserModel Creator { get; set; }
        public long CreationTimestamp { get; set; }
        public long? DueTimestamp { get; set; }
        public UserTaskStatusModel Status { get; set; }
        public UserTaskPriority? Priority { get; set; }
        public IEnumerable<UserModel> AssignedUsers { get; set; }
    }
}
