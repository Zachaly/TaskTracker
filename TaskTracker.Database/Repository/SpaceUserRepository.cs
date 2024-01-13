using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TaskTracker.Database.Exception;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.SpaceUser;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Database.Repository
{
    public interface ISpaceUserRepository : IKeylessRepositoryBase<SpaceUser, SpaceUserModel, GetSpaceUserRequest>
    {
        Task DeleteByUserIdAndSpaceIdAsync(long userId, long spaceId);
    }

    public class SpaceUserRepository : KeylessRepositoryBase<SpaceUser, SpaceUserModel, GetSpaceUserRequest>, ISpaceUserRepository
    {
        public SpaceUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = SpaceUserExpressions.Model;
        }

        public override Task<IEnumerable<T>> GetAsync<T>(GetSpaceUserRequest request, Func<SpaceUser, T> selector)
        {
            var query = FilterWithRequest(_dbContext.Set<SpaceUser>(), request);

            query = OrderBy(query, request).Include(u => u.User);

            return Task.FromResult(AddPagination(query, request).Select(selector).AsEnumerable());
        }

        public Task DeleteByUserIdAndSpaceIdAsync(long userId, long spaceId)
        {
            var entity = _dbContext.Set<SpaceUser>().FirstOrDefault(u => u.UserId == userId && u.SpaceId == spaceId);

            if(entity is null)
            {
                throw new EntityNotFoundException(nameof(SpaceUser));
            }

            _dbContext.Set<SpaceUser>().Remove(entity);

            return _dbContext.SaveChangesAsync();
        }
    }
}
