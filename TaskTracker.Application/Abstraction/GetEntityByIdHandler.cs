using MediatR;
using TaskTracker.Database;
using TaskTracker.Domain;
using TaskTracker.Model;

namespace TaskTracker.Application.Abstraction
{
    public class GetEntityByIdQuery<TModel> : IRequest<TModel?>
        where TModel : IModel
    {
        public long Id { get; set; }
    }

    public abstract class GetEntityByIdHandler<TEntity, TModel, TGetRequest, TQuery> : IRequestHandler<TQuery, TModel?>
        where TEntity : class, IEntity
        where TModel : IModel
        where TQuery : GetEntityByIdQuery<TModel>
        where TGetRequest : PagedRequest
    {
        private readonly IRepositoryBase<TEntity, TModel, TGetRequest> _repository;

        protected GetEntityByIdHandler(IRepositoryBase<TEntity, TModel, TGetRequest> repository)
        {
            _repository = repository;
        }

        public Task<TModel?> Handle(TQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetByIdAsync(request.Id);
        }
    }
}
