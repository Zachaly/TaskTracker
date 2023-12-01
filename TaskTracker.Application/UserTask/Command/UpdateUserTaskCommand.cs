using Azure.Core;
using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateUserTaskCommand : UpdateUserTaskRequest, IUpdateEntityCommand
    {
    }

    public class UpdateUserTaskHandler : UpdateEntityHandler<UserTask, UserTaskModel, UpdateUserTaskCommand>
    {
        public UpdateUserTaskHandler(IUserTaskRepository userTaskRepository, IValidator<UpdateUserTaskCommand> validator)
            : base(userTaskRepository, validator)
        {
        }

        protected override void UpdateEntity(UserTask entity, UpdateUserTaskCommand command)
        {
            entity.Title = command.Title ?? entity.Title;
            entity.Description = command.Description ?? entity.Description;
            entity.DueTimestamp = command.DueTimestamp;
            entity.StatusId = command.StatusId ?? entity.StatusId;
            entity.Priority = command.Priority;
        }
    }
}
