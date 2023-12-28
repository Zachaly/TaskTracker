using System.Linq.Expressions;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;

namespace TaskTracker.Expressions
{
    public static class DocumentExpressions
    {
        public static Expression<Func<TaskTrackerDocument, DocumentModel>> Model { get; } = document => new DocumentModel
        {
            CreationTimestamp = document.CreationTimestamp,
            Id = document.Id,
            Creator = UserExpressions.Model.Compile().Invoke(document.Creator),
            Title = document.Title,
            Pages = document.Pages.AsQueryable().Select(DocumentPageExpressions.Model).AsEnumerable(),
        };
    }
}
