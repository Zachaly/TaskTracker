namespace TaskTracker.Model.SpaceUser.Request
{
    public class GetSpaceUserRequest : PagedRequest
    {
        public long? UserId { get; set; }
        public long? SpaceId { get; set; }
    }
}
