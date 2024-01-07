using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Application.Command
{
    public class DeleteDocumentPageByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteDocumentPageByIdHandler : DeleteEntityByIdHandler<TaskTrackerDocumentPage, DocumentPageModel,
        GetDocumentPageRequest, DeleteDocumentPageByIdCommand>
    {
        public DeleteDocumentPageByIdHandler(IDocumentPageRepository repository) : base(repository)
        {
        }
    }
}
