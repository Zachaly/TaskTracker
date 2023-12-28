using TaskTracker.Application.Abstraction;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Application
{
    public interface IDocumentPageFactory : IEntityFactory<TaskTrackerDocumentPage, AddDocumentPageRequest>
    {

    }

    public class DocumentPageFactory : IDocumentPageFactory
    {
        public TaskTrackerDocumentPage Create(AddDocumentPageRequest request)
            => new TaskTrackerDocumentPage
            {
                Content = request.Content,
                Title = request.Title,
                DocumentId = request.DocumentId,
                LastModifiedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
    }
}
