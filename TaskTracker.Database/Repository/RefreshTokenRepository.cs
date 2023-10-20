﻿using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.RefreshToken;

namespace TaskTracker.Database.Repository
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken, RefreshTokenModel>
    {
        Task UpdateAsync(RefreshToken refreshToken);
        Task<bool> CheckIfTokenExistsAsync(string token);
    }

    public class RefreshTokenRepository : RepositoryBase<RefreshToken, RefreshTokenModel>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = RefreshTokenExpressions.Model;
        }

        public Task<bool> CheckIfTokenExistsAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
