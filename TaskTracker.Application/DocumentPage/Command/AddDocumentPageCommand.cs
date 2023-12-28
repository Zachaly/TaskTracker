using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Application.Command
{
    public class AddDocumentPageCommand : AddDocumentPageRequest, IAddEntityCommand
    {
    }

    public class AddDocumentPageHandler : AddEntityHandler<TaskTrackerDocumentPage, DocumentPageModel, AddDocumentPageRequest,
        AddDocumentPageCommand>
    {
        public AddDocumentPageHandler(IDocumentPageRepository repository, IDocumentPageFactory entityFactory,
            IValidator<AddDocumentPageCommand> validator) : base(repository, entityFactory, validator)
        {
        }
    }
}
