﻿using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Database.Repository
{
    public interface ITaskStatusGroupRepository : IRepositoryBase<TaskStatusGroup, TaskStatusGroupModel, GetTaskStatusGroupRequest>
    {
    }

    public class TaskStatusGroupRepository : RepositoryBase<TaskStatusGroup, TaskStatusGroupModel, GetTaskStatusGroupRequest>,
        ITaskStatusGroupRepository
    {
        public TaskStatusGroupRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            ModelExpression = TaskStatusGroupExpressions.Model;
        }

        public override Task<TaskStatusGroupModel?> GetByIdAsync(long id)
        {
            var query = _dbContext.Set<TaskStatusGroup>()
                .Include(g => g.Statuses);

            return FilterById(query, id).Select(ModelExpression).FirstOrDefaultAsync();
        }
    }
}
