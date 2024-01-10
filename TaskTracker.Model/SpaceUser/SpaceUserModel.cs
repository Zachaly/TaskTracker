using TaskTracker.Model.User;
using TaskTracker.Model.UserSpace;

namespace TaskTracker.Model.SpaceUser
{
    public class SpaceUserModel : IModel
    {
        public UserModel? User { get; set; }
        public UserSpaceModel? Space { get; set; }
    }
}
