using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.SpaceUserPermissions;
using TaskTracker.Model.SpaceUserPermissions.Request;

namespace TaskTracker.Application.Command
{
    public class AddSpaceUserPermissionsCommand : AddSpaceUserPermissionsRequest, IAddKeylessEntityCommand
    {
    }

    public class AddSpaceUserPermissionsHandler : AddKeylessEntityHandler<SpaceUserPermissions, SpaceUserPermissionsModel,
        GetSpaceUserPermissionsRequest, AddSpaceUserPermissionsRequest, AddSpaceUserPermissionsCommand>
    {
        public AddSpaceUserPermissionsHandler(ISpaceUserPermissionsRepository repository, ISpaceUserPermissionsFactory entityFactory,
            IValidator<AddSpaceUserPermissionsCommand> validator) : base(repository, entityFactory, validator)
        {
        }
    }
}
