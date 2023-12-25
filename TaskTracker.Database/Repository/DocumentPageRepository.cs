using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Database.Repository
{
    public interface IDocumentPageRepository : IRepositoryBase<TaskTrackerDocumentPage, DocumentPageModel, GetDocumentPageRequest>
    {

    }

    public class DocumentPageRepository : RepositoryBase<TaskTrackerDocumentPage, DocumentPageModel, GetDocumentPageRequest>,
        IDocumentPageRepository
    {
        public DocumentPageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
