using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;
using TaskTracker.Model.Document.Request;

namespace TaskTracker.Application.Command
{
    public class GetDocumentByIdQuery : GetEntityByIdQuery<DocumentModel>
    {
    }

    public class GetDocumentByIdHandler : GetEntityByIdHandler<TaskTrackerDocument, DocumentModel,
        GetDocumentRequest, GetDocumentByIdQuery>
    {
        public GetDocumentByIdHandler(IDocumentRepository repository) : base(repository)
        {
        }
    }
}
