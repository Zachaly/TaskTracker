using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.User;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserByIdQuery : GetEntityByIdQuery<UserModel>
    {
    }

    public class GetUserByIdHandler : GetEntityByIdHandler<User, UserModel, GetUserRequest, GetUserByIdQuery>
    {
        public GetUserByIdHandler(IUserRepository userRepository) : base(userRepository)
        {
            
        }
    }
}
