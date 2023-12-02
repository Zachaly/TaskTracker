using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateTaskListCommand : UpdateTaskListRequest, IUpdateEntityCommand
    {
    }

    public class UpdateTaskListHandler : UpdateEntityHandler<TaskList, TaskListModel, UpdateTaskListCommand>
    {
        public UpdateTaskListHandler(ITaskListRepository taskListRepository, IValidator<UpdateTaskListCommand> validator)
            : base(taskListRepository, validator)
        {
        }

        protected override void UpdateEntity(TaskList entity, UpdateTaskListCommand command)
        {
            entity.Description = command.Description ?? entity.Description;
            entity.Title = command.Title ?? entity.Title;
            entity.Color = command.Color ?? entity.Color;
        }
    }
}
