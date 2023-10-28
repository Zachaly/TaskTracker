using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using TaskTracker.Database.Exception;
using TaskTracker.Domain;
using TaskTracker.Model;
using TaskTracker.Model.Attribute;
using TaskTracker.Model.Enum;

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

        protected IQueryable<TEntity> FilterWithRequest<TRequest>(IQueryable<TEntity> queryable, TRequest request)
        {
            ImmutableList<(string Name, CustomFilterAttribute? Attr)> requestProps = typeof(TRequest).GetProperties()
                .Where(p => p.Name != "PageIndex" && p.Name != "PageSize")
                .Where(p => p.GetValue(request) is not null)
                .Select(p =>  (p.Name, p.GetCustomAttribute<CustomFilterAttribute>())).ToImmutableList();

            var entityProps = typeof(TEntity).GetProperties().Select(p => p.Name).ToImmutableList();

            var props = requestProps.Where(p => entityProps.Contains(p.Name) ||
                (p.Attr is not null && entityProps.Contains(p.Attr.PropertyName)))
                .ToImmutableList();

            var entityParam = Expression.Parameter(typeof(TEntity), "entity");

            foreach (var prop in props)
            {
                var entityPropName = prop.Attr?.PropertyName ?? prop.Name;
                var entityProp = typeof(TEntity).GetProperty(entityPropName);
                var underlayingType = Nullable.GetUnderlyingType(entityProp.PropertyType);
                var requestProp = typeof(TRequest).GetProperty(prop.Name);

                Expression entityPropExpression = Expression.Property(entityParam, entityPropName);

                if(underlayingType is not null)
                {
                    entityPropExpression = Expression.Convert(entityPropExpression, underlayingType);
                }

                var requestPropExpression = Expression.Constant(requestProp.GetValue(request));

                var filterAttribute = requestProp.GetCustomAttribute<CustomFilterAttribute>();

                BinaryExpression comparisonExpression = null;

                if (filterAttribute is not null)
                {
                    comparisonExpression = filterAttribute.ComparisonType switch
                    {
                        ComparisonType.Lesser => Expression.LessThan(entityPropExpression, requestPropExpression),
                        ComparisonType.LesserOrEqual => Expression.LessThanOrEqual(entityPropExpression, requestPropExpression),
                        ComparisonType.Greater => Expression.GreaterThan(entityPropExpression, requestPropExpression),
                        ComparisonType.GreaterOrEqual => Expression.GreaterThanOrEqual(entityPropExpression, requestPropExpression),
                        _ => throw new NotSupportedException()
                    };
                }
                else
                {
                    comparisonExpression = Expression.Equal(entityPropExpression, requestPropExpression);
                }

                var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(comparisonExpression,
                        entityParam);

                queryable = queryable.Where(lambdaExpression);
            }

            var t = queryable.ToList();

            return queryable;
        }

        protected IQueryable<TEntity> AddPagination(IQueryable<TEntity> queryable, PagedRequest request)
        {
            var index = request.PageIndex ?? 0;
            var pageSize = request.PageSize ?? 10;

            return queryable.Skip(index * pageSize).Take(pageSize);
        }
    }
}
