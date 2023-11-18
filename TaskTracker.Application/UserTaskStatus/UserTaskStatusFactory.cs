using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application
{
    public interface IUserTaskStatusFactory
    {
        UserTaskStatus Create(AddUserTaskStatusRequest request, bool isRemovable = false);
    }

    public class UserTaskStatusFactory : IUserTaskStatusFactory
    {
        public UserTaskStatus Create(AddUserTaskStatusRequest request, bool isRemovable = false)
        {
            throw new NotImplementedException();
        }
    }
}
