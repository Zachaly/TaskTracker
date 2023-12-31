﻿using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application
{
    public interface IUserTaskFactory : IEntityFactory<UserTask, AddUserTaskRequest>
    {
    }

    public class UserTaskFactory : IUserTaskFactory
    {
        public UserTask Create(AddUserTaskRequest request)
            => new UserTask
            {
                CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                CreatorId = request.CreatorId,
                Description = request.Description,
                DueTimestamp = request.DueTimestamp,
                Title = request.Title,
                ListId = request.ListId,
                StatusId = request.StatusId
            };
    }
}
