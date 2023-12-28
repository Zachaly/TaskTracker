using TaskTracker.Application;
using TaskTracker.Model.DocumentPage.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class DocumentPageFactoryTests
    {
        private readonly DocumentPageFactory _factory;

        public DocumentPageFactoryTests()
        {
            _factory = new DocumentPageFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddDocumentPageRequest
            {
                Content = "con",
                Title = "Title",
                DocumentId = 1
            };

            var page = _factory.Create(request);

            Assert.Equal(request.Title, page.Title);
            Assert.Equal(request.Content, page.Content);
            Assert.Equal(request.DocumentId, page.DocumentId);
            Assert.NotEqual(default, page.LastModifiedTimestamp);
        }
    }
}
