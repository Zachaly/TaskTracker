using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;

namespace TaskTracker.Application.Command
{
    public class DeleteDocumentPageByIdCommand : DeleteEntityByIdCommand
    {
    }

    public class DeleteDocumentPageByIdHandler : DeleteEntityByIdHandler<TaskTrackerDocumentPage, DocumentPageModel,
        DeleteDocumentPageByIdCommand>
    {
        public DeleteDocumentPageByIdHandler(IDocumentPageRepository repository) : base(repository)
        {
        }
    }
}
