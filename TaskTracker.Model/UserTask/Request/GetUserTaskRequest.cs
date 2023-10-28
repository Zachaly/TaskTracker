using TaskTracker.Model.Attribute;
using TaskTracker.Model.Enum;

namespace TaskTracker.Model.UserTask.Request
{
    public class GetUserTaskRequest : PagedRequest
    {
        public long? CreatorId { get; set; }
        [CustomFilter(ComparisonType = ComparisonType.LesserOrEqual)]
        public long? MaxDueTimestamp { get; set; }
        [CustomFilter(ComparisonType = ComparisonType.GreaterOrEqual)]
        public long? MinCreationTimestamp { get; set; }
    }
}
