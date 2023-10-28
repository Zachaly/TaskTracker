using TaskTracker.Model.User;

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
    }
}
