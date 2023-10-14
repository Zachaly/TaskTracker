using MediatR;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.User;

namespace TaskTracker.Application.Authorization.Command
{
    public class LoginCommand : IRequest<DataResponseModel<LoginResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, DataResponseModel<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUserRepository userRepository, IHashService hashService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        public async Task<DataResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if(user is null)
            {
                return new DataResponseModel<LoginResponse>("Invalid password or email");
            }

            if(!await _hashService.CompareStringWithHashAsync(request.Password, user.PasswordHash))
            {
                return new DataResponseModel<LoginResponse>("Invalid password or email");
            }

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            await _userRepository.UpdateAsync(user);

            return new DataResponseModel<LoginResponse>(new LoginResponse
            {
                UserData = new UserModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                },
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
