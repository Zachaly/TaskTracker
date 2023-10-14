using TaskTracker.Domain.Entity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using TaskTracker.Application.Authorization.Exception;

namespace TaskTracker.Application.Authorization.Service
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);
        Task<string> GenerateRefreshToken();
        Task<long> GetUserIdFromAccessToken(string accessToken);

    }

    public class TokenService : ITokenService
    {
        private readonly string _authIssuer;
        private readonly string _authAudience;
        private readonly string _secretKey;

        public TokenService(IConfiguration configuration)
        {
            _authIssuer = configuration["Auth:Issuer"]!;
            _authAudience = configuration["Auth:Audience"]!;
            _secretKey = configuration["Auth:SecretKey"]!;
        }

        public Task<string> GenerateAccessToken(User user)
        {
            var claims = new List<Claim> 
            { 
                new Claim("sub", user.Id.ToString()),
                new Claim("name", user.FirstName)
            };

            var handler = new JsonWebTokenHandler();

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = _authAudience,
                Issuer = _authIssuer,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(GetRsaSecurityKey(), SecurityAlgorithms.Sha256),
                Expires = DateTime.UtcNow.AddHours(1),
                NotBefore = DateTime.UtcNow
            });

            return Task.FromResult(token);
        }

        public Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Task.FromResult(Convert.ToBase64String(randomNumber));
            }
        }

        public Task<long> GetUserIdFromAccessToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                IssuerSigningKey = GetRsaSecurityKey(),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAlgorithms = new[] { SecurityAlgorithms.Sha256 }
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

            if(securityToken is not JwtSecurityToken)
            {
                throw new InvalidTokenException("Token is not valid jwt");
            }

            var idClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "sub");

            if(idClaim is null)
            {
                throw new InvalidTokenException("Token lacks required claim");
            }

            var id = long.Parse(idClaim.Value);

            return Task.FromResult(id);
        }

        private RsaSecurityKey GetRsaSecurityKey()
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Encoding.UTF8.GetBytes(_secretKey), out _);

            return new RsaSecurityKey(rsa);
        }
    }
}
