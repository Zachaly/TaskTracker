using Microsoft.EntityFrameworkCore;
using TaskTracker.Database.Exception;
using TaskTracker.Domain.Entity;
using TaskTracker.Expressions;
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
            ModelExpression = DocumentExpressions.Model;
        }

        public override Task DeleteByIdAsync(long id)
        {
            var entity = _dbContext.Set<TaskTrackerDocument>()
                .Include(d => d.Pages)
                .FirstOrDefault(x => x.Id == id);

            if(entity is null)
            {
                throw new EntityNotFoundException(nameof(TaskTrackerDocument));
            }

            _dbContext.Set<TaskTrackerDocument>().Remove(entity);

            return _dbContext.SaveChangesAsync();
        }
    }
}
