using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application
{
    public interface ITaskStatusGroupFactory
    {
        TaskStatusGroup Create(AddTaskStatusGroupRequest request, bool isRemovable = false);
    }

    public class TaskStatusGroupFactory : ITaskStatusGroupFactory
    {
        public TaskStatusGroup Create(AddTaskStatusGroupRequest request, bool isRemovable = false)
        {
            throw new NotImplementedException();
        }
    }
}
