using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using TaskTracker.Application;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Document.Request;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class DocumentCommandTests
    {
        [Fact]
        public async Task AddDocumentCommand_ValidRequest_AddsDocumentWithEmptyPage()
        {
            var documents = new List<TaskTrackerDocument>();
            var pages = new List<TaskTrackerDocumentPage>();

            var command = new AddDocumentCommand
            {
                CreatorId = 1,
                SpaceId = 2,
                Title = "title",
            };

            var documentFactory = Substitute.For<IDocumentFactory>();

            documentFactory.Create(command).Returns(info => new TaskTrackerDocument
            {
                CreatorId = info.Arg<AddDocumentRequest>().CreatorId
            });

            var documentPageFactory = Substitute.For<IDocumentPageFactory>();

            documentPageFactory.Create(Arg.Any<AddDocumentPageRequest>()).Returns(info => new TaskTrackerDocumentPage
            {
                DocumentId = info.Arg<AddDocumentPageRequest>().DocumentId
            });

            var documentRepository = Substitute.For<IDocumentRepository>();

            const long NewId = 5;

            documentRepository.AddAsync(Arg.Any<TaskTrackerDocument>())
                .Returns(NewId)
                .AndDoes(info => documents.Add(info.Arg<TaskTrackerDocument>()));

            var documentPageRepository = Substitute.For<IDocumentPageRepository>();

            documentPageRepository.AddAsync(Arg.Any<TaskTrackerDocumentPage>())
                .Returns(0)
                .AndDoes(info => pages.Add(info.Arg<TaskTrackerDocumentPage>()));

            var validator = Substitute.For<IValidator<AddDocumentCommand>>();

            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new AddDocumentHandler(documentRepository, documentFactory, validator,
                documentPageRepository, documentPageFactory);

            var res = await handler.Handle(command, default);

            await documentPageRepository.Received(1).AddAsync(Arg.Any<TaskTrackerDocumentPage>());

            Assert.True(res.IsSuccess);
            Assert.Contains(documents, d => d.CreatorId == command.CreatorId);
            Assert.Contains(pages, d => d.DocumentId == NewId);
        }

        [Fact]
        public async Task AddDocumentCommand_InvalidRequest_Failure()
        {
            var command = new AddDocumentCommand();

            var documentFactory = Substitute.For<IDocumentFactory>();

            var documentPageFactory = Substitute.For<IDocumentPageFactory>();

            var documentRepository = Substitute.For<IDocumentRepository>();

            var documentPageRepository = Substitute.For<IDocumentPageRepository>();

            var validator = Substitute.For<IValidator<AddDocumentCommand>>();

            validator.ValidateAsync(command).Returns(new ValidationResult
            {
                Errors = new List<ValidationFailure>()
                {
                    new ValidationFailure("prop", "err")
                }
            });

            var handler = new AddDocumentHandler(documentRepository, documentFactory, validator,
                documentPageRepository, documentPageFactory);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.NotNull(res.ValidationErrors);
        }
    }
}
