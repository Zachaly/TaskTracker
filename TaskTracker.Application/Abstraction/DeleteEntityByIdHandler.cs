﻿using MediatR;
using TaskTracker.Database;
using TaskTracker.Database.Exception;
using TaskTracker.Domain;
using TaskTracker.Model;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Abstraction
{
    public class DeleteEntityByIdCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public abstract class DeleteEntityByIdHandler<TEntity, TModel, TGetRequest, TCommand> : IRequestHandler<TCommand, ResponseModel>
        where TCommand : DeleteEntityByIdCommand
        where TEntity : class, IEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        protected readonly IRepositoryBase<TEntity, TModel, TGetRequest> _repository;

        protected DeleteEntityByIdHandler(IRepositoryBase<TEntity, TModel, TGetRequest> repository)
        {
            _repository = repository;
        }

        public virtual async Task<ResponseModel> Handle(TCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteByIdAsync(request.Id);

                return new ResponseModel();
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }
        }
    }
}
