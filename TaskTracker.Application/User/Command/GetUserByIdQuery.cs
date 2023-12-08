using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.User;

namespace TaskTracker.Application.Command
{
    public class GetUserByIdQuery : GetEntityByIdQuery<UserModel>
    {
    }

    public class GetUserByIdHandler : GetEntityByIdHandler<User, UserModel, GetUserByIdQuery>
    {
        public GetUserByIdHandler(IUserRepository userRepository) : base(userRepository)
        {
            
        }
    }
}
