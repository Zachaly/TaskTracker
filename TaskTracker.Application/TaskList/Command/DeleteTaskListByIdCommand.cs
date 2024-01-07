using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class DeleteTaskListByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteTaskListByIdHandler : DeleteEntityByIdHandler<TaskList, TaskListModel,
        GetTaskListRequest, DeleteTaskListByIdCommand>
    {
        public DeleteTaskListByIdHandler(ITaskListRepository taskListRepository) : base(taskListRepository) 
        {
        }
    }
}
