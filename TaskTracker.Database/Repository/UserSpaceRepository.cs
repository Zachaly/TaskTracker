using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.UserSpace;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Database.Repository
{
    public interface IUserSpaceRepository : IRepositoryBase<UserSpace, UserSpaceModel, GetUserSpaceRequest>
    {

    }

    public class UserSpaceRepository : RepositoryBase<UserSpace, UserSpaceModel, GetUserSpaceRequest>, IUserSpaceRepository
    {
        public UserSpaceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = UserSpaceExpressions.Model;
        }
    }
}
