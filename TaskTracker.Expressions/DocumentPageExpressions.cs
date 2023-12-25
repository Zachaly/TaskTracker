using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;

namespace TaskTracker.Expressions
{
    public static class DocumentPageExpressions
    {
        public static Expression<Func<TaskTrackerDocumentPage, DocumentPageModel>> Model { get; } = page => new DocumentPageModel
        {
            Content = page.Content,
            Id = page.Id,
            LastModifiedTimestamp = page.LastModifiedTimestamp,
            Title = page.Title,
        };
    }
}
