using TaskTracker.Model.Attribute;
using TaskTracker.Model.Enum;

namespace TaskTracker.Model.SpaceUser.Request
{
    public class GetSpaceUserRequest : PagedRequest
    {
        public long? UserId { get; set; }
        public long? SpaceId { get; set; }
        [CustomFilter(ComparisonType = ComparisonType.DoesNotContain, PropertyName = "UserId")]
        public ICollection<long>? SkipUserIds { get; set; }
    }
}
