using TaskTracker.Domain.Enum;

namespace TaskTracker.Model.UserTask.Request
{
    public class UpdateUserTaskRequest
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long? DueTimestamp { get; set; }
        public long? StatusId { get; set; }
        public UserTaskPriority? Priority { get; set; }
    }
}
