using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;
using TaskTracker.Model.Document.Request;

namespace TaskTracker.Database.Repository
{
    public interface IDocumentRepository : IRepositoryBase<TaskTrackerDocument, DocumentModel, GetDocumentRequest>
    {

    }

    public class DocumentRepository : RepositoryBase<TaskTrackerDocument, DocumentModel, GetDocumentRequest>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
