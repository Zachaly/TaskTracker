using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Application.Command
{
    public class GetDocumentPageByIdQuery : GetEntityByIdQuery<DocumentPageModel>
    {
    }

    public class GetDocumentPageByIdHandler : GetEntityByIdHandler<TaskTrackerDocumentPage, DocumentPageModel,
        GetDocumentPageRequest, GetDocumentPageByIdQuery>
    {
        public GetDocumentPageByIdHandler(IDocumentPageRepository repository) : base(repository)
        {
        }
    }
}
