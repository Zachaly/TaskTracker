using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.User;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserQuery : GetUserRequest, IGetEntityQuery<UserModel>
    {
    }

    public class GetUserHandler : GetEntityHandler<User, UserModel, GetUserRequest, GetUserQuery>
    {
        public GetUserHandler(IUserRepository repository) : base(repository)
        {
        }
    }
}
