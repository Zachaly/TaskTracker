using FluentValidation;
using MediatR;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Application.Command
{
    public class ChangeUserPasswordCommand : ChangeUserPasswordRequest, IRequest<ResponseModel>
    {
    }

    public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordCommand, ResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IValidator<ChangeUserPasswordCommand> _validator;
        private readonly IHashService _hashService;

        public ChangeUserPasswordHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository,
            IValidator<ChangeUserPasswordCommand> validator, IHashService hashService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _validator = validator;
            _hashService = hashService;
        }

        public async Task<ResponseModel> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, x => x);

            if(user is null)
            {
                return new ResponseModel("User not found");
            }

            if(!await _hashService.CompareStringWithHashAsync(request.CurrentPassword, user.PasswordHash))
            {
                return new ResponseModel("Invalid password");
            }

            var newPasswordHash = await _hashService.HashStringAsync(request.NewPassword);

            user.PasswordHash = newPasswordHash;

            await _userRepository.UpdateAsync(user);

            var tokens = (await _refreshTokenRepository.GetByUserIdAsync(user.Id)).ToList();

            foreach(var token in tokens)
            {
                token.IsValid = false;
                await _refreshTokenRepository.UpdateAsync(token);
            }

            return new ResponseModel();
        }
    }
}
