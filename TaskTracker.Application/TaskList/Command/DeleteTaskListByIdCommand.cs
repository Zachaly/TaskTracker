using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;

namespace TaskTracker.Application.Command
{
    public class DeleteTaskListByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteTaskListByIdHandler : DeleteEntityByIdHandler<TaskList, TaskListModel, DeleteTaskListByIdCommand>
    {
        public DeleteTaskListByIdHandler(ITaskListRepository taskListRepository) : base(taskListRepository) 
        {
        }
    }
}
