using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateUserSpaceCommand : UpdateUserSpaceRequest, IUpdateEntityCommand
    {
    }

    public class UpdateUserSpaceHandler : UpdateEntityHandler<UserSpace, UserSpaceModel, GetUserSpaceRequest, UpdateUserSpaceCommand>
    {
        public UpdateUserSpaceHandler(IUserSpaceRepository repository, IValidator<UpdateUserSpaceCommand> validator) 
            : base(repository, validator)
        {
        }

        protected override void UpdateEntity(UserSpace entity, UpdateUserSpaceCommand command)
        {
            entity.Title = command.Title ?? entity.Title;
        }
    }
}
