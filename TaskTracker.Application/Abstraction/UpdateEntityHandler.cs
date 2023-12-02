using FluentValidation;
using MediatR;
using TaskTracker.Database;
using TaskTracker.Domain;
using TaskTracker.Model;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Abstraction
{
    public interface IUpdateEntityCommand : IRequest<ResponseModel>
    {
        long Id { get; set; }
    }

    public abstract class UpdateEntityHandler<TEntity, TModel, TCommand> : IRequestHandler<TCommand, ResponseModel>
        where TEntity : class, IEntity
        where TModel : IModel
        where TCommand : IUpdateEntityCommand
    {
        private readonly IRepositoryBase<TEntity, TModel> _repository;
        private readonly IValidator<TCommand> _validator;

        protected UpdateEntityHandler(IRepositoryBase<TEntity, TModel> repository, IValidator<TCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var entity = await _repository.GetByIdAsync(request.Id, x => x);

            if(entity is null)
            {
                return new ResponseModel("Entity not found");
            }

            UpdateEntity(entity, request);

            await _repository.UpdateAsync(entity);

            return new ResponseModel();
        }

        protected abstract void UpdateEntity(TEntity entity, TCommand command);
    }
}
