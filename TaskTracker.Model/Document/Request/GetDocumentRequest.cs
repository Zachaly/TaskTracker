namespace TaskTracker.Model.Document.Request
{
    public class GetDocumentRequest : PagedRequest
    {
        public long? CreatorId { get; set; }
        public long? SpaceId { get; set; }
    }
}
