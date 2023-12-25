namespace TaskTracker.Model.DocumentPage.Request
{
    public class GetDocumentPageRequest : PagedRequest
    {
        public long? DocumentId { get; set; }
    }
}
