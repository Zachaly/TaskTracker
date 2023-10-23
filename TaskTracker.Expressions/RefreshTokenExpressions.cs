using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.RefreshToken;

namespace TaskTracker.Expressions
{
    public static class RefreshTokenExpressions
    {
        public static Expression<Func<RefreshToken, RefreshTokenModel>> Model { get; } = token => new RefreshTokenModel
            {
                Token = token.Token,
                CreationDate = token.CreationDate,
                ExpiryDate = token.ExpiryDate,
                UserId = token.UserId,
            };
    }
}
