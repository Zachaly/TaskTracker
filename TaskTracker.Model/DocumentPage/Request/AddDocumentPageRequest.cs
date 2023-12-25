namespace TaskTracker.Model.DocumentPage.Request
{
    public class AddDocumentPageRequest
    {
        public long DocumentId { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
    }
}
