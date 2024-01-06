using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.SpaceUser.Request;

namespace TaskTracker.Application.Command
{
    public class AddSpaceUserCommand : AddSpaceUserRequest, IRequest<ResponseModel>
    {
    }

    public class AddSpaceUserHandler : IRequestHandler<AddSpaceUserCommand, ResponseModel>
    {
        private readonly ISpaceUserRepository _repository;
        private readonly IValidator<AddSpaceUserCommand> _validator;
        private readonly ISpaceUserFactory _factory;

        public AddSpaceUserHandler(ISpaceUserRepository repository, ISpaceUserFactory factory, IValidator<AddSpaceUserCommand> validator)
        {
            _repository = repository;
            _validator = validator;
            _factory = factory;
        }

        public async Task<ResponseModel> Handle(AddSpaceUserCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var user = _factory.Create(request);

            await _repository.AddAsync(user);

            return new ResponseModel();
        }
    }
}
