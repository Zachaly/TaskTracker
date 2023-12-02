using FluentValidation;
using MediatR;
using TaskTracker.Database;
using TaskTracker.Domain;
using TaskTracker.Model;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Abstraction
{
    public interface IAddEntityCommand : IRequest<CreatedResponseModel>
    {
    }

    public abstract class AddEntityHandler<TEntity, TModel, TAddRequest, TCommand> : IRequestHandler<TCommand, CreatedResponseModel>
        where TEntity : class, IEntity
        where TModel : IModel
        where TCommand : IAddEntityCommand, TAddRequest
    {
        protected readonly IRepositoryBase<TEntity, TModel> _repository;
        protected readonly IEntityFactory<TEntity, TAddRequest> _entityFactory;
        protected readonly IValidator<TCommand> _validator;

        protected AddEntityHandler(IRepositoryBase<TEntity, TModel> repository, IEntityFactory<TEntity, TAddRequest> entityFactory,
            IValidator<TCommand> validator)
        {
            _repository = repository;
            _entityFactory = entityFactory;
            _validator = validator;
        }

        public virtual async Task<CreatedResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var entity = _entityFactory.Create(request);

            var newId = await _repository.AddAsync(entity);

            return new CreatedResponseModel(newId);
        }
    }
}
