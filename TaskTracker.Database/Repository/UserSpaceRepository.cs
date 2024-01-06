using Microsoft.EntityFrameworkCore;
using TaskTracker.Database.Exception;
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

        public override Task<IEnumerable<UserSpaceModel>> GetAsync(GetUserSpaceRequest request)
        {
            var query = FilterWithRequest(_dbContext.Set<UserSpace>(), request)
                .Include(s => s.StatusGroup)
                .ThenInclude(g => g.Statuses)
                .Include(s => s.Lists)
                .ThenInclude(l => l.Creator)
                .Include(s => s.Documents)
                .ThenInclude(s => s.Pages);

            return Task.FromResult(AddPagination(query, request).Select(ModelExpression).AsEnumerable());
        }

        public override Task<UserSpaceModel?> GetByIdAsync(long id)
        {
            var query = FilterById(_dbContext.Set<UserSpace>(), id)
                .Include(s => s.StatusGroup)
                .ThenInclude(g => g.Statuses)
                .Include(s => s.Lists)
                .ThenInclude(l => l.Creator);

            return query.Select(ModelExpression).FirstOrDefaultAsync();
        }

        public override async Task DeleteByIdAsync(long id)
        {
            var space = await _dbContext.Set<UserSpace>()
                .Include(s => s.Lists)
                .ThenInclude(s => s.Tasks)
                .Include(s => s.Users)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(space is null)
            {
                throw new EntityNotFoundException(nameof(UserSpace));
            }

            _dbContext.Set<UserSpace>().Remove(space);

            await _dbContext.SaveChangesAsync();
        }
    }
}
