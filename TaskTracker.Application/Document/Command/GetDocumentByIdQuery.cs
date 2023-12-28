using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;

namespace TaskTracker.Application.Command
{
    public class GetDocumentByIdQuery : GetEntityByIdQuery<DocumentModel>
    {
    }

    public class GetDocumentByIdHandler : GetEntityByIdHandler<TaskTrackerDocument, DocumentModel, GetDocumentByIdQuery>
    {
        public GetDocumentByIdHandler(IDocumentRepository repository) : base(repository)
        {
        }
    }
}
