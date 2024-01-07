using FluentValidation;
using MediatR;
using TaskTracker.Database;
using TaskTracker.Domain;
using TaskTracker.Model;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Abstraction
{
    public interface IAddKeylessEntityCommand : IRequest<ResponseModel> { }

    public abstract class AddKeylessEntityHandler<TEntity, TModel, TGetRequest, TAddRequest, TCommand> : IRequestHandler<TCommand, ResponseModel>
        where TEntity : class, IKeylessEntity
        where TModel : IModel
        where TCommand : IAddKeylessEntityCommand, TAddRequest
        where TGetRequest : PagedRequest
    {
        protected readonly IKeylessRepositoryBase<TEntity, TModel, TGetRequest> _repository;
        protected readonly IEntityFactory<TEntity, TAddRequest> _entityFactory;
        protected readonly IValidator<TCommand> _validator;

        protected AddKeylessEntityHandler(IKeylessRepositoryBase<TEntity, TModel, TGetRequest> repository,
            IEntityFactory<TEntity, TAddRequest> entityFactory,
            IValidator<TCommand> validator)
        {
            _repository = repository;
            _entityFactory = entityFactory;
            _validator = validator;
        }

        public virtual async Task<ResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var entity = _entityFactory.Create(request);

            await _repository.AddAsync(entity);

            return new ResponseModel();
        }
    }
}
