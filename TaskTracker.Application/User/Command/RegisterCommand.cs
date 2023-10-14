using FluentValidation;
using MediatR;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Application.Command
{
    public class RegisterCommand : RegisterRequest, IRequest<CreatedResponseModel>
    {
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CreatedResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly IValidator<RegisterCommand> _validator;
        private readonly IUserFactory _userFactory;

        public RegisterCommandHandler(IUserRepository userRepository, IHashService hashService,
            IUserFactory userFactory, IValidator<RegisterCommand> validator)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _validator = validator;
            _userFactory = userFactory;
        }
        public async Task<CreatedResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if((await _userRepository.GetByEmailAsync(request.Email)) is not null)
            {
                return new CreatedResponseModel("Email already taken");
            }

            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var user = _userFactory.Create(request, await _hashService.HashStringAsync(request.Password));

            await _userRepository.AddAsync(user);

            return new CreatedResponseModel(user.Id);
        }
    }
}
