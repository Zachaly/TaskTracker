using TaskTracker.Model.Attribute;
using TaskTracker.Model.Enum;

namespace TaskTracker.Model.UserTask.Request
{
    public class GetUserTaskRequest : PagedRequest
    {
        public long? CreatorId { get; set; }
        [CustomFilter(ComparisonType = ComparisonType.LesserOrEqual, PropertyName = "DueTimestamp")]
        public long? MaxDueTimestamp { get; set; }
        [CustomFilter(ComparisonType = ComparisonType.GreaterOrEqual, PropertyName = "CreationTimestamp")]
        public long? MinCreationTimestamp { get; set; }

        public long? ListId { get; set; }

        public long? StatusId { get; set; }

        [CustomFilter(ComparisonType = ComparisonType.Contains, PropertyName = "StatusId")]
        public ICollection<long>? StatusIds { get; set; }

        [CustomFilter(ComparisonType = ComparisonType.DoesNotContain, PropertyName = "StatusId")]
        public ICollection<long>? SkipStatusIds { get; set; }
    }
}
