namespace TaskTracker.Model.SpaceUserPermissions.Request
{
    public class GetSpaceUserPermissionsRequest : PagedRequest
    {
        public long? SpaceId { get; set; }
        public long? UserId { get; set; }
    }
}
