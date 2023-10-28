using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application
{
    public interface IUserTaskFactory
    {
        UserTask Create(AddUserTaskRequest request);
    }

    public class UserTaskFactory : IUserTaskFactory
    {
        public UserTask Create(AddUserTaskRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
