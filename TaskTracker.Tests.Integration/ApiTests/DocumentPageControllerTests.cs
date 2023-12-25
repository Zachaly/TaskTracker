using System.Net;
using System.Net.Http.Json;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.DocumentPage.Request;
using TaskTracker.Model.Response;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class DocumentPageControllerTests : ApiTest
    {
        const string Endpoint = "api/document-page";

        [Fact]
        public async Task GetAsync_ReturnsPages()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var pages = FakeDataFactory.GenerateDocumentPages(5, document.Id);

            _dbContext.DocumentPages.AddRange(pages);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<DocumentPageModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(pages.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEntity()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var page = FakeDataFactory.GenerateDocumentPages(1, document.Id).First();

            _dbContext.DocumentPages.Add(page);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"{Endpoint}/{page.Id}");

            var content = await response.Content.ReadFromJsonAsync<DocumentPageModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(page.Title, content.Title);
            Assert.Equal(page.Content, content.Content);
            Assert.Equal(page.Id, content.Id);
            Assert.Equal(page.LastModifiedTimestamp, content.LastModifiedTimestamp);
        }

        [Fact]
        public async Task GetByIdAsync_NoPageWithSpecifiedIdFound_ReturnsNotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ValidRequest_AddsNewDocumentPage()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var request = new AddDocumentPageRequest
            {
                DocumentId = document.Id,
                Content = "text",
                Title = "title"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.DocumentPages, page => page.Id == content.NewEntityId
                && page.Content == request.Content
                && page.Title == request.Title
                && page.DocumentId == request.DocumentId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new AddDocumentPageRequest
            {
                DocumentId = -1,
                Content = "con",
                Title = "Title",
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<CreatedResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "DocumentId");
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_UpdatesCorrectEntity()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var page = FakeDataFactory.GenerateDocumentPages(1, document.Id).First();

            _dbContext.DocumentPages.Add(page);
            _dbContext.SaveChanges();

            var request = new UpdateDocumentPageRequest
            {
                Id = page.Id,
                Content = "new content",
                Title = "new title"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var originalModifiedTimestamp = page.LastModifiedTimestamp;

            await _dbContext.Entry(page).ReloadAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Title, page.Title);
            Assert.Equal(request.Content, page.Content);
            Assert.NotEqual(originalModifiedTimestamp, page.LastModifiedTimestamp);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_ReturnsBadRequest()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var page = FakeDataFactory.GenerateDocumentPages(1, document.Id).First();

            _dbContext.DocumentPages.Add(page);
            _dbContext.SaveChanges();

            var request = new UpdateDocumentPageRequest
            {
                Id = page.Id,
                Content = "new content",
                Title =new string('a', 201)
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var originalModifiedTimestamp = page.LastModifiedTimestamp;

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            await _dbContext.Entry(page).ReloadAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(request.Title, page.Title);
            Assert.NotEqual(request.Content, page.Content);
            Assert.Equal(originalModifiedTimestamp, page.LastModifiedTimestamp);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Title");
        }

        [Fact]
        public async Task UpdateAsync_EntityNotFound_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateDocumentPageRequest
            {
                Id = 2137,
                Content = "new content",
                Title = "new title"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesCorrectEntity()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var page = FakeDataFactory.GenerateDocumentPages(1, document.Id).First();

            _dbContext.DocumentPages.Add(page);
            _dbContext.SaveChanges();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{page.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.DocumentPages, p => p.Id == page.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_PageWithSpecifiedIdNotFound_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
