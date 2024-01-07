using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;
using TaskTracker.Model.Document.Request;

namespace TaskTracker.Application.Command
{
    public class DeleteDocumentByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteDocumentByIdHandler : DeleteEntityByIdHandler<TaskTrackerDocument, DocumentModel,
        GetDocumentRequest, DeleteDocumentByIdCommand>
    {
        public DeleteDocumentByIdHandler(IDocumentRepository repository) : base(repository)
        {
        }
    }
}
