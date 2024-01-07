using TaskTracker.Domain;

namespace TaskTracker.Application.Abstraction
{
    public interface IEntityFactory<out TEntity, in TAddRequest>
        where TEntity : IKeylessEntity
    {
        TEntity Create(TAddRequest request);
    }
}
