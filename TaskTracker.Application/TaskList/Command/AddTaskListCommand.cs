using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class AddTaskListCommand : AddTaskListRequest, IAddEntityCommand
    {
    }

    public class AddTaskListHandler : AddEntityHandler<TaskList, TaskListModel, AddTaskListRequest, AddTaskListCommand>
    {
        public AddTaskListHandler(ITaskListRepository taskListRepository, ITaskListFactory taskListFactory,
            IValidator<AddTaskListCommand> validator) : base(taskListRepository, taskListFactory, validator)
        {
        }
    }
}
