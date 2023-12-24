namespace TaskTracker.Model.UserSpace.Request
{
    public class GetUserSpaceRequest : PagedRequest
    {
        public long? OwnerId { get; set; }
        public long? StatusGroupId { get; set; }
    }
}
