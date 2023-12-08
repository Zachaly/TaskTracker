using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.User;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateUserCommand : UpdateUserRequest, IUpdateEntityCommand
    {
    }

    public class UpdateUserHandler : UpdateEntityHandler<User, UserModel, UpdateUserCommand>
    {
        public UpdateUserHandler(IUserRepository userRepository, IValidator<UpdateUserCommand> validator) 
            : base(userRepository, validator)
        {
            
        }

        protected override void UpdateEntity(User entity, UpdateUserCommand command)
        {
            entity.FirstName = command.FirstName ?? entity.FirstName;
            entity.LastName = command.LastName ?? entity.LastName;
        }
    }
}
