using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using TaskTracker.Domain;
using TaskTracker.Model;
using TaskTracker.Model.Attribute;
using TaskTracker.Model.Enum;

namespace TaskTracker.Database
{
    public interface IKeylessRepositoryBase<TEntity, TModel, TGetRequest> 
        where TEntity : class, IKeylessEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task AddAsync(TEntity entity);
        Task AddAsync(params TEntity[] entities);
        Task<IEnumerable<TModel>> GetAsync(TGetRequest request);
        Task<IEnumerable<T>> GetAsync<T>(TGetRequest request, Func<TEntity, T> selector);
    }

    public abstract class KeylessRepositoryBase<TEntity, TModel, TGetRequest> :  IKeylessRepositoryBase<TEntity, TModel, TGetRequest>
        
        where TEntity : class, IKeylessEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        protected readonly ApplicationDbContext _dbContext;
        protected Expression<Func<TEntity, TModel>> ModelExpression { get; set; }

        protected KeylessRepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);

            return _dbContext.SaveChangesAsync();
        }

        public Task AddAsync(params TEntity[] entities)
        {
            _dbContext.AddRange(entities);

            return _dbContext.SaveChangesAsync();
        }

        public virtual Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            var query = FilterWithRequest(_dbContext.Set<TEntity>(), request);

            query = OrderBy(query, request);

            return Task.FromResult(AddPagination(query, request).Select(ModelExpression).AsEnumerable());
        }

        public virtual Task<IEnumerable<T>> GetAsync<T>(TGetRequest request, Func<TEntity, T> selector)
        {
            var query = FilterWithRequest(_dbContext.Set<TEntity>(), request);

            query = OrderBy(query, request);

            return Task.FromResult(AddPagination(query, request).Select(selector).AsEnumerable());
        }

        protected IQueryable<TEntity> FilterWithRequest<TRequest>(IQueryable<TEntity> queryable, TRequest request)
        {
            ImmutableList<(string Name, CustomFilterAttribute? Attr)> requestProps = typeof(TRequest).GetProperties()
                .Where(p => p.Name != "PageIndex" && p.Name != "PageSize" && p.Name != "OrderBy" && p.GetCustomAttribute<JoinAttribute>() is null)
                .Where(p => p.GetValue(request) is not null)
                .Select(p => (p.Name, p.GetCustomAttribute<CustomFilterAttribute>())).ToImmutableList();

            var entityProps = typeof(TEntity).GetProperties().Select(p => p.Name).ToImmutableList();

            var props = requestProps.Where(p => entityProps.Contains(p.Name) ||
                (p.Attr is not null && entityProps.Contains(p.Attr.PropertyName)))
                .ToImmutableList();

            var entityParam = Expression.Parameter(typeof(TEntity), "entity");

            foreach (var prop in props)
            {
                var entityPropName = prop.Attr?.PropertyName ?? prop.Name;
                var entityProp = typeof(TEntity).GetProperty(entityPropName)!;
                var underlayingType = Nullable.GetUnderlyingType(entityProp.PropertyType);
                var requestProp = typeof(TRequest).GetProperty(prop.Name)!;

                Expression entityPropExpression = Expression.Property(entityParam, entityPropName);

                if (underlayingType is not null)
                {
                    entityPropExpression = Expression.Convert(entityPropExpression, underlayingType);
                }

                var requestPropExpression = Expression.Constant(requestProp.GetValue(request));

                var filterAttribute = requestProp.GetCustomAttribute<CustomFilterAttribute>();

                Expression comparisonExpression;

                if (filterAttribute is not null)
                {
                    var methodInfo = requestProp.PropertyType.GetMethod("Contains");

                    comparisonExpression = filterAttribute.ComparisonType switch
                    {
                        ComparisonType.Lesser => Expression.LessThan(entityPropExpression, requestPropExpression),
                        ComparisonType.LesserOrEqual => Expression.LessThanOrEqual(entityPropExpression, requestPropExpression),
                        ComparisonType.Greater => Expression.GreaterThan(entityPropExpression, requestPropExpression),
                        ComparisonType.GreaterOrEqual => Expression.GreaterThanOrEqual(entityPropExpression, requestPropExpression),
                        ComparisonType.Contains => Expression.Call(requestPropExpression, methodInfo!, entityPropExpression),
                        ComparisonType.DoesNotContain => Expression.Equal(Expression.Call(requestPropExpression, methodInfo!, entityPropExpression), Expression.Constant(false)),
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

            return queryable;
        }

        protected IQueryable<TEntity> AddPagination(IQueryable<TEntity> queryable, PagedRequest request)
        {
            if (request.SkipPagination.GetValueOrDefault())
            {
                return queryable;
            }

            var index = request.PageIndex ?? 0;
            var pageSize = request.PageSize ?? 10;

            return queryable.Skip(index * pageSize).Take(pageSize);
        }

        protected IQueryable<TEntity> OrderBy(IQueryable<TEntity> queryable, PagedRequest request)
        {
            if (request.OrderBy is null && request.OrderByDescending is null)
            {
                return queryable;
            }

            var propertyName = request.OrderBy is null ? request.OrderByDescending : request.OrderBy;

            var methodName = request.OrderBy is null ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, queryable.Expression, Expression.Quote(orderByExp));

            return queryable.Provider.CreateQuery<TEntity>(resultExp);
        }
    }
}
