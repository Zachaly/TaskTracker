using Microsoft.EntityFrameworkCore;
using TaskTracker.Database.Exception;
using TaskTracker.Domain;
using TaskTracker.Model;

namespace TaskTracker.Database
{

    public interface IRepositoryBase<TEntity, TModel, TGetRequest> : IKeylessRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : class, IEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task<TModel?> GetByIdAsync(long id);
        Task<T> GetByIdAsync<T>(long id, Func<TEntity, T> selector);
        new Task<long> AddAsync(TEntity entity);
        Task DeleteByIdAsync(long id);
        Task UpdateAsync(TEntity entity);
    }

    public abstract class RepositoryBase<TEntity, TModel, TGetRequest> : KeylessRepositoryBase<TEntity, TModel, TGetRequest>,
        IRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : class, IEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        protected RepositoryBase(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task<long> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public virtual Task<TModel?> GetByIdAsync(long id)
        {
            var query = FilterById(_dbContext.Set<TEntity>(), id);

            return query.Select(ModelExpression).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByIdAsync<T>(long id, Func<TEntity, T> selector)
        {
            var query = FilterById(_dbContext.Set<TEntity>(), id);

            return Task.FromResult(query.Select(selector).FirstOrDefault());
        }

        public virtual async Task DeleteByIdAsync(long id)
        {
            TEntity entity;

            try
            {
                entity = await _dbContext.Set<TEntity>().FirstAsync(e => e.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(nameof(TEntity));
            }

            _dbContext.Set<TEntity>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);

            return _dbContext.SaveChangesAsync();
        }

        protected IQueryable<TEntity> FilterById(IQueryable<TEntity> entities, long id)
            => entities.Where(e => e.Id == id);
    }
}
