using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document.Request;

namespace TaskTracker.Application
{
    public interface IDocumentFactory : IEntityFactory<TaskTrackerDocument, AddDocumentRequest>
    {

    }

    public class DocumentFactory : IDocumentFactory
    {
        public TaskTrackerDocument Create(AddDocumentRequest request)
            => new TaskTrackerDocument
            {
                CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                CreatorId = request.CreatorId,
                SpaceId = request.SpaceId,
                Title = request.Title,
            };
    }
}
