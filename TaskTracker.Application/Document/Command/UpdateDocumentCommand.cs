using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document;
using TaskTracker.Model.Document.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateDocumentCommand : UpdateDocumentRequest, IUpdateEntityCommand
    {
    }

    public class UpdateDocumentHandler : UpdateEntityHandler<TaskTrackerDocument, DocumentModel,
        GetDocumentRequest, UpdateDocumentCommand>
    {
        public UpdateDocumentHandler(IDocumentRepository repository, IValidator<UpdateDocumentCommand> validator) : base(repository, validator)
        {
        }

        protected override void UpdateEntity(TaskTrackerDocument entity, UpdateDocumentCommand command)
        {
            entity.Title = command.Title ?? entity.Title;
        }
    }
}
