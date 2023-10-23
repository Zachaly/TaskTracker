using MediatR;
using TaskTracker.Application.Authorization.Exception;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.User;

namespace TaskTracker.Application.Command
{
    public class RefreshTokenCommand : IRequest<DataResponseModel<LoginResponse>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, DataResponseModel<LoginResponse>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenFactory _refreshTokenFactory;

        public RefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService,
            IRefreshTokenFactory refreshTokenFactory)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _refreshTokenFactory = refreshTokenFactory;
        }

        public async Task<DataResponseModel<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetTokenAsync(request.RefreshToken);

            if(refreshToken is null)
            {
                return new DataResponseModel<LoginResponse>("Invalid token");
            }

            long userId;

            try
            {
                userId = await _tokenService.GetUserIdFromAccessTokenAsync(request.AccessToken);
            }
            catch(InvalidTokenException ex)
            {
                return new DataResponseModel<LoginResponse>(ex.Message);
            }

            if(userId != refreshToken.UserId)
            {
                return new DataResponseModel<LoginResponse>("Invalid token");
            }

            refreshToken.IsValid = false;

            await _refreshTokenRepository.UpdateAsync(refreshToken);

            var newAccessToken = await _tokenService.GenerateAccessTokenAsync(refreshToken.User);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();

            while(await _refreshTokenRepository.GetTokenAsync(newRefreshToken) is not null)
            {
                newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();
            }

            await _refreshTokenRepository.AddAsync(_refreshTokenFactory.Create(userId, newRefreshToken));

            return new DataResponseModel<LoginResponse>(new LoginResponse
            {
                UserData = new UserModel
                {
                    Email = refreshToken.User.Email,
                    FirstName = refreshToken.User.FirstName,
                    Id = refreshToken.User.Id,
                    LastName = refreshToken.User.LastName
                },
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
