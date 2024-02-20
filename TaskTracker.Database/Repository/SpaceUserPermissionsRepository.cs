using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.SpaceUserPermissions;
using TaskTracker.Model.SpaceUserPermissions.Request;

namespace TaskTracker.Database.Repository
{
    public interface ISpaceUserPermissionsRepository : IKeylessRepositoryBase<SpaceUserPermissions, SpaceUserPermissionsModel,
        GetSpaceUserPermissionsRequest>
    {
        Task<SpaceUserPermissions> GetBySpaceIdAndUserIdAsync(long spaceId, long userId);
        Task UpdateAsync(SpaceUserPermissions entity);
    }

    public class SpaceUserPermissionsRepository : KeylessRepositoryBase<SpaceUserPermissions, SpaceUserPermissionsModel,
        GetSpaceUserPermissionsRequest>, ISpaceUserPermissionsRepository
    {
        public SpaceUserPermissionsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = SpaceUserPermissionsExpressions.Model;
        }

        public Task<SpaceUserPermissions?> GetBySpaceIdAndUserIdAsync(long spaceId, long userId)
            => _dbContext.Set<SpaceUserPermissions>().FirstOrDefaultAsync(p => p.SpaceId == spaceId && p.UserId == userId);

        public Task UpdateAsync(SpaceUserPermissions entity)
        {
            _dbContext.Set<SpaceUserPermissions>().Update(entity);

            return _dbContext.SaveChangesAsync();
        }
    }
}
