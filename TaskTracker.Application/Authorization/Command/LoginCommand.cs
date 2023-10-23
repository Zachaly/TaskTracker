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
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenFactory _refreshTokenFactory;

        public LoginCommandHandler(IUserRepository userRepository, IHashService hashService, ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository, IRefreshTokenFactory refreshTokenFactory)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenFactory = refreshTokenFactory;
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

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshTokenString = await _tokenService.GenerateRefreshTokenAsync();

            while(await _refreshTokenRepository.GetTokenAsync(refreshTokenString) is not null)
            {
                refreshTokenString = await _tokenService.GenerateRefreshTokenAsync();
            }

            var refreshToken = _refreshTokenFactory.Create(user.Id, refreshTokenString);

            await _refreshTokenRepository.AddAsync(refreshToken);

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
                RefreshToken = refreshTokenString
            });
        }
    }
}
