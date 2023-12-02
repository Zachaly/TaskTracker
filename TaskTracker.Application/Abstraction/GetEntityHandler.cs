using MediatR;
using TaskTracker.Database;
using TaskTracker.Domain;
using TaskTracker.Model;

namespace TaskTracker.Application.Abstraction
{
    public interface IGetEntityQuery<TModel> : IRequest<IEnumerable<TModel>>
        where TModel : IModel
    {

    }

    public abstract class GetEntityHandler<TEntity, TModel, TGetRequest, TQuery> : IRequestHandler<TQuery, IEnumerable<TModel>>
        where TGetRequest : PagedRequest
        where TEntity : class, IEntity
        where TModel : IModel
        where TQuery : IGetEntityQuery<TModel>, TGetRequest
    {
        protected readonly IRepositoryBase<TEntity, TModel, TGetRequest> _repository;

        protected GetEntityHandler(IRepositoryBase<TEntity, TModel, TGetRequest> repository)
        {
            _repository = repository;
        }

        public virtual Task<IEnumerable<TModel>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAsync(request);
        }
    }
}
