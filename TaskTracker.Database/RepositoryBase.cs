using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskTracker.Database.Exception;
using TaskTracker.Domain;
using TaskTracker.Model;

namespace TaskTracker.Database
{
    public interface IRepositoryBase<TEntity, TModel>
        where TEntity : class, IEntity
        where TModel : IModel
    {
        Task<TModel?> GetByIdAsync(long id);
        Task<T> GetByIdAsync<T>(long id, Func<TEntity, T> selector);
        Task<long> AddAsync(TEntity entity);
        Task DeleteByIdAsync(long id);
    }

    public abstract class RepositoryBase<TEntity, TModel> : IRepositoryBase<TEntity, TModel>
        where TEntity :  class, IEntity
        where TModel : IModel
    {
        protected readonly ApplicationDbContext _dbContext;

        protected Expression<Func<TEntity, TModel>> ModelExpression { get; set; }

        protected RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task DeleteByIdAsync(long id)
        {
            TEntity entity;

            try
            {
                entity = await _dbContext.Set<TEntity>().FirstAsync(e => e.Id == id);
            }
            catch(InvalidOperationException)
            {
                throw new EntityNotFoundException(nameof(TEntity));
            }

            _dbContext.Set<TEntity>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        protected IQueryable<TModel> GetModels(IQueryable<TEntity> entities)
            => entities.Select(ModelExpression);

        protected IQueryable<TEntity> FilterById(IQueryable<TEntity> entities, long id)
            => entities.Where(e => e.Id == id);

        public virtual Task<TModel?> GetByIdAsync(long id)
        {
            var query = FilterById(_dbContext.Set<TEntity>(), id);

            return GetModels(query).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByIdAsync<T>(long id, Func<TEntity, T> selector)
        {
            var query = FilterById(_dbContext.Set<TEntity>(), id);

            return Task.FromResult(query.Select(selector).FirstOrDefault());
        }
    }
}
