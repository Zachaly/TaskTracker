using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application
{
    public interface ITaskListFactory
    {
        TaskList Create(AddTaskListRequest request);
    }

    public class TaskListFactory : ITaskListFactory
    {
        public TaskList Create(AddTaskListRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
