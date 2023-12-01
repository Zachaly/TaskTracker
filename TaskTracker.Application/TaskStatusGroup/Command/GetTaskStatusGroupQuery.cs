using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskStatusGroupQuery : GetTaskStatusGroupRequest, IGetEntityQuery<TaskStatusGroupModel>
    {
    }

    public class GetTaskStatusGroupHandler : GetEntityHandler<TaskStatusGroup, TaskStatusGroupModel,
        GetTaskStatusGroupRequest, GetTaskStatusGroupQuery>
    { 
        public GetTaskStatusGroupHandler(ITaskStatusGroupRepository taskStatusGroupRepository)
            : base(taskStatusGroupRepository) { }
    }
}
