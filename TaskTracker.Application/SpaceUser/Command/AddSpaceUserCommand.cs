using FluentValidation;
using MediatR;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.SpaceUser;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Application.Command
{
    public class AddSpaceUserCommand : AddSpaceUserRequest, IAddKeylessEntityCommand
    {
    }

    public class AddSpaceUserHandler : AddKeylessEntityHandler<SpaceUser, SpaceUserModel, GetSpaceUserRequest,
        AddSpaceUserRequest, AddSpaceUserCommand>
    {

        public AddSpaceUserHandler(ISpaceUserRepository repository, ISpaceUserFactory factory, IValidator<AddSpaceUserCommand> validator)
            : base(repository, factory, validator)
        {
        }
    }
}
