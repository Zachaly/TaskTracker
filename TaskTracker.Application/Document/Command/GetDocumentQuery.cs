using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;
using TaskTracker.Model.Document.Request;

namespace TaskTracker.Application.Command
{
    public class GetDocumentQuery : GetDocumentRequest, IGetEntityQuery<DocumentModel>
    {
    }

    public class GetDocumentHandler : GetEntityHandler<TaskTrackerDocument, DocumentModel, GetDocumentRequest, GetDocumentQuery>
    {
        public GetDocumentHandler(IDocumentRepository repository) : base(repository)
        {
        }
    }
}
