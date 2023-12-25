using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Model.Document;
using TaskTracker.Model.Response;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class DocumentControllerTests : ApiTest
    {
        const string Endpoint = "api/document";

        [Fact]
        public async Task GetAsync_ReturnsDocuments()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroup.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var documents = FakeDataFactory.GenerateDocuments(5, space.Id, userId);

            _dbContext.Documents.AddRange(documents);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<DocumentModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(documents.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProperEntity()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroup.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var pages = FakeDataFactory.GenerateDocumentPages(1, document.Id);

            _dbContext.DocumentPages.AddRange(pages);
            _dbContext.SaveChanges();

            var user = _dbContext.Users.First(x => x.Id == userId);

            var response = await _httpClient.GetAsync($"{Endpoint}/{document.Id}");

            var content = await response.Content.ReadFromJsonAsync<DocumentModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(document.Title, content.Title);
            Assert.Equal(document.CreationTimestamp, content.CreationTimestamp);
            Assert.Equal(user.FirstName, content.Creator.FirstName);
            Assert.Equal(user.LastName, content.Creator.LastName);
            Assert.Equal(user.Email, content.Creator.Email);
            Assert.Equal(user.Id, content.Creator.Id);
            Assert.Equivalent(pages.Select(x => x.Id), content.Pages.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_NoDocumentWithSpecifiedId_ReturnsNotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ValidRequest_DocumentWithEmptyPageAdded()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroup.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var request = new AddDocumentCommand
            {
                CreatorId = userId,
                SpaceId = space.Id,
                Title = "title"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.Documents.Include(d => d.Pages), doc => doc.Id == content.NewEntityId
                && doc.Title == request.Title
                && doc.SpaceId == request.SpaceId
                && doc.CreatorId == request.CreatorId
                && doc.Pages.Any());
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new AddDocumentCommand
            {
                CreatorId = -1,
                SpaceId = 1,
                Title = "title"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<CreatedResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "CreatorId");
        }

        [Fact]
        public async Task UpdateAsync_ValidRequest_UpdatesCorrectEntity()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroup.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var request = new UpdateDocumentCommand
            {
                Id = document.Id,
                Title = "new title"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            await _dbContext.Entry(document).ReloadAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Title, document.Title);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_ReturnsBadRequest()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroup.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var request = new UpdateDocumentCommand
            {
                Id = document.Id,
                Title = new string('a', 1000)
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            await _dbContext.Entry(document).ReloadAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(request.Title, document.Title);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Title");
        }

        [Fact]
        public async Task UpdateAsync_EntityNotFound_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateDocumentCommand
            {
                Id = 2137,
                Title = "new title"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesCorrectEntityWithChildren()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroup.Id).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var document = FakeDataFactory.GenerateDocuments(1, space.Id, userId).First();

            _dbContext.Documents.Add(document);
            _dbContext.SaveChanges();

            var pages = FakeDataFactory.GenerateDocumentPages(1, document.Id);

            _dbContext.DocumentPages.AddRange(pages);
            _dbContext.SaveChanges();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{document.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.DocumentPages, p => p.DocumentId == document.Id);
            Assert.DoesNotContain(_dbContext.Documents, d => d.Id == document.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_NoDocumentWithSpecifiedId_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
