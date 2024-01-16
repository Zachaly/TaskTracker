using TaskTracker.Model.Attribute;
using TaskTracker.Model.Enum;

namespace TaskTracker.Model.User.Request
{
    public class GetUserRequest : PagedRequest
    {
        [CustomFilter(ComparisonType = ComparisonType.DoesNotContain, PropertyName = "Id")]
        public ICollection<long>? SkipIds { get; set; }

        [CustomFilter(ComparisonType = ComparisonType.StartsWith, PropertyName = "Email")]
        public string? SearchEmail { get; set; }

        [CustomFilter(ComparisonType = ComparisonType.Contains, PropertyName = "Id")]
        public ICollection<long>? Ids { get; set; }
    }
}
