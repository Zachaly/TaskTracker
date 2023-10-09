namespace TaskTracker.Domain
{
    public interface IEntity : IKeylessEntity
    {
        public long Id { get; set; }
    }
}
