using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Application
{
    public interface ISpaceUserFactory
    {
        SpaceUser Create(AddSpaceUserRequest request);
    }

    public class SpaceUserFactory : ISpaceUserFactory
    {
        public SpaceUser Create(AddSpaceUserRequest request)
            => new SpaceUser
            {
                SpaceId = request.SpaceId,
                UserId = request.UserId,
            };
    }
}
