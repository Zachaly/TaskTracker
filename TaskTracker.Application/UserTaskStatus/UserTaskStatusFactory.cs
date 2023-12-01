using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application
{
    public interface IUserTaskStatusFactory : IEntityFactory<UserTaskStatus, AddUserTaskStatusRequest>
    {
        UserTaskStatus Create(AddUserTaskStatusRequest request, bool isDefault);
    }

    public class UserTaskStatusFactory : IUserTaskStatusFactory
    {
        public UserTaskStatus Create(AddUserTaskStatusRequest request, bool isDefault)
            => new UserTaskStatus
            {
                Color = request.Color,
                GroupId = request.GroupId,
                Index = request.Index,
                IsDefault = isDefault,
                Name = request.Name,
            };

        public UserTaskStatus Create(AddUserTaskStatusRequest request)
            => new UserTaskStatus
            {
                Color = request.Color,
                GroupId = request.GroupId,
                Index = request.Index,
                IsDefault = false,
                Name = request.Name,
            };
    }
}
