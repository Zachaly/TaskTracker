using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Application
{
    public interface ISpaceUserFactory : IEntityFactory<SpaceUser, AddSpaceUserRequest>
    {
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
