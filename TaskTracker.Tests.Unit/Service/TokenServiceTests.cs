using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskTracker.Application.Authorization.Exception;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Unit.Service
{
    public class TokenServiceTests
    {
        private readonly TokenService _service;
        private readonly IConfiguration _configuration;

        public TokenServiceTests()
        {
            _configuration = Substitute.For<IConfiguration>();

            _configuration["Auth:Issuer"].Returns("issuer");
            _configuration["Auth:Audience"].Returns("audience");
            _configuration["Auth:SecretKey"].Returns(new string('a', 64));

            _service = new TokenService(_configuration);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ReturnsValidToken()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "name"
            };

            var token = await _service.GenerateAccessTokenAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.MapInboundClaims = false;

            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _configuration["Auth:Audience"],
                ValidIssuer = _configuration["Auth:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:SecretKey"]!)),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256Signature },
                
            };

            var claims = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            Assert.IsType<JwtSecurityToken>(validatedToken);
            Assert.Contains(claims.Claims, x => x.Type == "sub" && x.Value == user.Id.ToString());
        }

        [Fact]
        public async Task GetUserIdFromAccessTokenAsync_ReturnsUserId()
        {
            var user = new User
            {
                Id = 1,
                FirstName = "name"
            };

            var token = await _service.GenerateAccessTokenAsync(user);

            var id = await _service.GetUserIdFromAccessTokenAsync(token);

            Assert.Equal(user.Id, id);
        }

        [Fact]
        public async Task GetUserIdFromAccessTokenAsync_TokenWithoutClaim_ThrowsException()
        {
            var tokenHandler = new JsonWebTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = _configuration["Auth:Audience"],
                Issuer = _configuration["Auth:Issuer"],
                Subject = new ClaimsIdentity(new List<Claim>()),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:SecretKey"])),
                SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddHours(1),
                NotBefore = DateTime.UtcNow,
            });

            await Assert.ThrowsAsync<InvalidTokenException>(() => _service.GetUserIdFromAccessTokenAsync(token));
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_ReturnsRandomString()
        {
            var str1 = await _service.GenerateRefreshTokenAsync();
            var str2 = await _service.GenerateRefreshTokenAsync();

            Assert.NotEqual(str1, str2);
        }
    }
}
