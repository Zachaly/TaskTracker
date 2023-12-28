using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;

namespace TaskTracker.Application.Command
{
    public class DeleteDocumentByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteDocumentByIdHandler : DeleteEntityByIdHandler<TaskTrackerDocument, DocumentModel, DeleteDocumentByIdCommand>
    {
        public DeleteDocumentByIdHandler(IDocumentRepository repository) : base(repository)
        {
        }
    }
}
