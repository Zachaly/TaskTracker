using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Application.DocumentPage.Command
{
    public class UpdateDocumentPageCommand : UpdateDocumentPageRequest, IUpdateEntityCommand
    {
    }

    public class UpdateDocumentPageHandler : UpdateEntityHandler<TaskTrackerDocumentPage, DocumentPageModel,
        GetDocumentPageRequest, UpdateDocumentPageCommand>
    {
        public UpdateDocumentPageHandler(IDocumentPageRepository repository, IValidator<UpdateDocumentPageCommand> validator) : base(repository, validator)
        {
        }

        protected override void UpdateEntity(TaskTrackerDocumentPage entity, UpdateDocumentPageCommand command)
        {
            entity.Title = command.Title;
            entity.Content = command.Content ?? entity.Content;
            entity.LastModifiedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
