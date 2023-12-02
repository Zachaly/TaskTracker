using TaskTracker.Domain;

namespace TaskTracker.Application.Abstraction
{
    public interface IEntityFactory<out TEntity, in TAddRequest>
        where TEntity : IEntity
    {
        TEntity Create(TAddRequest request);
    }
}
