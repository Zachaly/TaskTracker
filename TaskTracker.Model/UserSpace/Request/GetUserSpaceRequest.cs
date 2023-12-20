using TaskTracker.Model.Attribute;

namespace TaskTracker.Model.UserSpace.Request
{
    public class GetUserSpaceRequest : PagedRequest
    {
        public long? OwnerId { get; set; }
        public long? StatusGroupId { get; set; }
        [Join]
        public bool? JoinLists { get; set; }
    }
}
