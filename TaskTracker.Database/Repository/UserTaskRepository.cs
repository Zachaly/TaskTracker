﻿using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Database.Repository
{
    public interface IUserTaskRepository : IRepositoryBase<UserTask, UserTaskModel>
    {
        Task<IEnumerable<UserTaskModel>> GetAsync(GetUserTaskRequest request);
        Task UpdateAsync(UserTask task);
    }

    public class UserTaskRepository : RepositoryBase<UserTask, UserTaskModel>, IUserTaskRepository
    {
        public UserTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = UserTaskExpressions.Model;
        }

        public Task<IEnumerable<UserTaskModel>> GetAsync(GetUserTaskRequest request)
        {
            var filteredQueryable = FilterWithRequest(_dbContext.Set<UserTask>(), request);

            return Task.FromResult(AddPagination(filteredQueryable, request).Select(ModelExpression).AsEnumerable());
        }

        public override Task<UserTaskModel?> GetByIdAsync(long id)
        {
            var queryable = _dbContext.Set<UserTask>()
                .Include(t => t.Creator);
            
            return FilterById(queryable, id).Select(ModelExpression).FirstOrDefaultAsync();
        }

        public Task UpdateAsync(UserTask task)
        {
            _dbContext.Set<UserTask>().Update(task);

            return _dbContext.SaveChangesAsync();
        }
    }
}
