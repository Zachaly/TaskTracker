using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application
{
    public interface ITaskListFactory : IEntityFactory<TaskList, AddTaskListRequest>
    {
    }

    public class TaskListFactory : ITaskListFactory
    {
        public TaskList Create(AddTaskListRequest request)
            => new TaskList
            {
                Color = request.Color,
                CreatorId = request.CreatorId,
                Description = request.Description,
                Title = request.Title,
                TaskStatusGroupId = request.StatusGroupId
            };
    }
}
