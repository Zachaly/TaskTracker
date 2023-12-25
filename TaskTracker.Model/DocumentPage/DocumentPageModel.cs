namespace TaskTracker.Model.DocumentPage
{
    public class DocumentPageModel : IModel
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
        public long LastModifiedTimestamp { get; set; }
    }
}
