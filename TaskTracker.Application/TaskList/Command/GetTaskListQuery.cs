using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskListQuery : GetTaskListRequest, IGetEntityQuery<TaskListModel>
    {
    }

    public class GetTaskListHandler : GetEntityHandler<TaskList, TaskListModel, GetTaskListRequest, GetTaskListQuery>
    {
        public GetTaskListHandler(ITaskListRepository repository) : base(repository)
        {
        }
    }
}
