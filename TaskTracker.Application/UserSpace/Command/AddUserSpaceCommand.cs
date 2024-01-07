using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Application.Command
{
    public class AddUserSpaceCommand : AddUserSpaceRequest, IAddEntityCommand
    {
    }


    public class AddUserSpaceHandler : AddEntityHandler<UserSpace, UserSpaceModel,
        GetUserSpaceRequest, AddUserSpaceRequest, AddUserSpaceCommand>
    {
        public AddUserSpaceHandler(IUserSpaceRepository repository, IUserSpaceFactory entityFactory,
            IValidator<AddUserSpaceCommand> validator) : base(repository, entityFactory, validator)
        {
        }
    }
}
