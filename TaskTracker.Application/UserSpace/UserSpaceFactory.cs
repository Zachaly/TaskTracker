using System;
using System.Collections.Generic;
using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Application
{
    public interface IUserSpaceFactory : IEntityFactory<UserSpace, AddUserSpaceRequest>
    {

    }

    public class UserSpaceFactory : IUserSpaceFactory
    {
        public UserSpace Create(AddUserSpaceRequest request)
            => new UserSpace
            {
                OwnerId = request.UserId,
                StatusGroupId = request.StatusGroupId,
                Title = request.Title,
            };
    }
}
