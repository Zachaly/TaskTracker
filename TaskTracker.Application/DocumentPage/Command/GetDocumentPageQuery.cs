using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Application.Command
{
    public class GetDocumentPageQuery : GetDocumentPageRequest, IGetEntityQuery<DocumentPageModel>
    {
    }

    public class GetDocumentPageHandler : GetEntityHandler<TaskTrackerDocumentPage, DocumentPageModel, GetDocumentPageRequest,
        GetDocumentPageQuery>
    {
        public GetDocumentPageHandler(IDocumentPageRepository repository) : base(repository)
        {
        }
    }
}
