namespace TaskTracker.Model.DocumentPage.Request
{
    public class UpdateDocumentPageRequest
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
