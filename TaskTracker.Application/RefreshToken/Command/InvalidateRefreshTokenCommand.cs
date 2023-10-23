using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public class InvalidateRefreshTokenCommand : IRequest<ResponseModel>
    {
        public string RefreshToken { get; set; }
    }

    public class InvalidateRefreshTokenHandler : IRequestHandler<InvalidateRefreshTokenCommand, ResponseModel>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public InvalidateRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<ResponseModel> Handle(InvalidateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository.GetTokenAsync(request.RefreshToken);

            if(token is null)
            {
                return new ResponseModel();
            }

            token.IsValid = false;

            await _refreshTokenRepository.UpdateAsync(token);

            return new ResponseModel();
        }
    }
}
