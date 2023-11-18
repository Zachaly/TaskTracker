using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application
{
    public interface ITaskStatusGroupFactory
    {
        TaskStatusGroup Create(AddTaskStatusGroupRequest request, bool isDefault = false);
    }

    public class TaskStatusGroupFactory : ITaskStatusGroupFactory
    {
        public TaskStatusGroup Create(AddTaskStatusGroupRequest request, bool isDefault = false)
        {
            throw new NotImplementedException();
        }
    }
}
