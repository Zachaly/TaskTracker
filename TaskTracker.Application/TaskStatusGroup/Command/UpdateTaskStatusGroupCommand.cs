using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateTaskStatusGroupCommand : UpdateTaskStatusGroupRequest, IUpdateEntityCommand
    {
    }

    public class UpdateTaskStatusGroupHandler : UpdateEntityHandler<TaskStatusGroup, TaskStatusGroupModel,
        GetTaskStatusGroupRequest, UpdateTaskStatusGroupCommand>
    {
        public UpdateTaskStatusGroupHandler(ITaskStatusGroupRepository taskStatusGroupRepository,
            IValidator<UpdateTaskStatusGroupCommand> validator)
            : base(taskStatusGroupRepository, validator)
        {
        }

        protected override void UpdateEntity(TaskStatusGroup entity, UpdateTaskStatusGroupCommand command)
        {
            entity.Name = command.Name ?? entity.Name;
        }
    }
}
