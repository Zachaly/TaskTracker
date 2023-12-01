using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup;

namespace TaskTracker.Application.Command
{
    public class GetTaskStatusGroupByIdQuery : GetEntityByIdQuery<TaskStatusGroupModel>
    {
    }

    public class GetTaskStatusGroupByIdHandler : GetEntityByIdHandler<TaskStatusGroup, TaskStatusGroupModel, GetTaskStatusGroupByIdQuery>
    {
        public GetTaskStatusGroupByIdHandler(ITaskStatusGroupRepository taskStatusGroupRepository)
            : base(taskStatusGroupRepository) 
        {
        }
    }
}
