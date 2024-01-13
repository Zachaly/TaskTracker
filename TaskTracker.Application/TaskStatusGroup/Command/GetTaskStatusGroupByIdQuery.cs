using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskStatusGroupByIdQuery : GetEntityByIdQuery<TaskStatusGroupModel>
    {
    }

    public class GetTaskStatusGroupByIdHandler : GetEntityByIdHandler<TaskStatusGroup, TaskStatusGroupModel,
        GetTaskStatusGroupRequest, GetTaskStatusGroupByIdQuery>
    {
        public GetTaskStatusGroupByIdHandler(ITaskStatusGroupRepository taskStatusGroupRepository)
            : base(taskStatusGroupRepository) 
        {
        }
    }
}
