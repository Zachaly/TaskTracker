namespace TaskTracker.Model.Document.Request
{
    public class AddDocumentRequest
    {
        public long CreatorId { get; set; }
        public long SpaceId { get; set; }
        public string Title { get; set; }
    }
}
