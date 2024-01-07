using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskListByIdQuery : GetEntityByIdQuery<TaskListModel>
    {
    }

    public class GetTaskListByIdHandler : GetEntityByIdHandler<TaskList, TaskListModel, GetTaskListRequest, GetTaskListByIdQuery>
    {
        public GetTaskListByIdHandler(ITaskListRepository taskListRepository) : base(taskListRepository)
        {
        }
    }
}
