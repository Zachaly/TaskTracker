using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;

namespace TaskTracker.Application.Command
{
    public class DeleteUserSpaceByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteUserSpaceByIdHandler : DeleteEntityByIdHandler<UserSpace, UserSpaceModel, DeleteUserSpaceByIdCommand>
    {
        public DeleteUserSpaceByIdHandler(IUserSpaceRepository repository) : base(repository)
        {
        }
    }
}
