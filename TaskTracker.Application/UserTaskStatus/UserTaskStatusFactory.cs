using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application
{
    public interface IUserTaskStatusFactory
    {
        UserTaskStatus Create(AddUserTaskStatusRequest request, bool isDefault = false);
    }

    public class UserTaskStatusFactory : IUserTaskStatusFactory
    {
        public UserTaskStatus Create(AddUserTaskStatusRequest request, bool isDefault = false)
            => new UserTaskStatus
            {
                Color = request.Color,
                GroupId = request.GroupId,
                Index = request.Index,
                IsDefault = isDefault,
                Name = request.Name,
            };
    }
}
