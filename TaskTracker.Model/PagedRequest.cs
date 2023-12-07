namespace TaskTracker.Model
{
    public abstract class PagedRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public bool? SkipPagination { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderByDescending { get; set; }
    }
}
