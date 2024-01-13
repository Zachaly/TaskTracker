using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Application.Command
{
    public class DeleteUserSpaceByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteUserSpaceByIdHandler : DeleteEntityByIdHandler<UserSpace, UserSpaceModel,
        GetUserSpaceRequest,DeleteUserSpaceByIdCommand>
    {
        public DeleteUserSpaceByIdHandler(IUserSpaceRepository repository) : base(repository)
        {
        }
    }
}
