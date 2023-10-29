namespace TaskTracker.Model
{
    public abstract class PagedRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
