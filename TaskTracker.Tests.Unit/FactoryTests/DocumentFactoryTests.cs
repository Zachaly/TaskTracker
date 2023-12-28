using TaskTracker.Application;
using TaskTracker.Model.Document.Request;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class DocumentFactoryTests
    {
        private readonly DocumentFactory _factory;

        public DocumentFactoryTests()
        {
            _factory = new DocumentFactory();
        }

        [Fact]
        public async Task Create_CreatesValidEntity()
        {
            var request = new AddDocumentRequest
            {
                CreatorId = 1,
                Title = "Test",
                SpaceId = 1,
            };

            var document = _factory.Create(request);

            Assert.Equal(request.CreatorId, document.CreatorId);
            Assert.Equal(request.Title, document.Title);
            Assert.Equal(request.SpaceId, document.SpaceId);
            Assert.NotEqual(default, document.CreationTimestamp);
        }
    }
}
