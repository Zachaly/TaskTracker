using System.Net;
using System.Net.Http.Json;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserSpace;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class UserSpaceControllerTests : ApiTest
    {
        const string Endpoint = "/api/user-space";

        [Fact]
        public async Task GetAsync_ReturnsSpaces()
        {
            var userData = await AuthorizeAsync();

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var spaces = FakeDataFactory.GenerateUserSpaces(5, userData.UserData.Id, statusGroup.Id);

            _dbContext.UserSpaces.AddRange(spaces);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserSpaceModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(spaces.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProperUserSpace()
        {
            var userData = await AuthorizeAsync();

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var spaces = FakeDataFactory.GenerateUserSpaces(5, userData.UserData.Id, statusGroup.Id);

            _dbContext.UserSpaces.AddRange(spaces);
            _dbContext.SaveChanges();

            var space = spaces.Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{space.Id}");

            var content = await response.Content.ReadFromJsonAsync<UserSpaceModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(space.Id, content.Id);
            Assert.Equal(space.Title, content.Title);
            Assert.Equal(space.OwnerId, content.Owner.Id);
            Assert.Equal(space.StatusGroupId, content.StatusGroup.Id);
        }

        [Fact]
        public async Task GetByIdAsync_NoSpaceWithSpecifiedId_ReturnsNotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ValidRequest_AddsNewSpace()
        {
            var userData = await AuthorizeAsync();

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var request = new AddUserSpaceRequest
            {
                StatusGroupId = statusGroup.Id,
                UserId = userData.UserData.Id,
                Title = "title"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.UserSpaces, space => space.Id == content.NewEntityId
                && space.OwnerId == request.UserId
                && space.StatusGroupId == request.StatusGroupId
                && space.Title == request.Title);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new AddUserSpaceRequest
            {
                StatusGroupId = 1,
                UserId = 1,
                Title = ""
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<CreatedResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Title");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProperSpace()
        {
            var userData = await AuthorizeAsync();

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var spaces = FakeDataFactory.GenerateUserSpaces(2, userData.UserData.Id, statusGroup.Id);

            _dbContext.UserSpaces.AddRange(spaces);
            _dbContext.SaveChanges();

            var updatedSpace = spaces.Last();

            var request = new UpdateUserSpaceRequest
            {
                Id = updatedSpace.Id,
                Title = "new title"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            await _dbContext.Entry(updatedSpace).ReloadAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Title, updatedSpace.Title);
        }

        [Fact]
        public async Task UpdateAsync_InvalidSpaceId_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateUserSpaceRequest
            {
                Id = 1,
                Title = "new title"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(content.Error);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_ReturnsBadRequest()
        {
            var userData = await AuthorizeAsync();

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var spaces = FakeDataFactory.GenerateUserSpaces(2, userData.UserData.Id, statusGroup.Id);

            _dbContext.UserSpaces.AddRange(spaces);
            _dbContext.SaveChanges();

            var updatedSpace = spaces.Last();

            var request = new UpdateUserSpaceRequest
            {
                Id = updatedSpace.Id,
                Title = ""
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            await _dbContext.Entry(updatedSpace).ReloadAsync();

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(request.Title, updatedSpace.Title);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Title");
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesProperEntity()
        {
            var userData = await AuthorizeAsync();

            var statusGroup = _dbContext.TaskStatusGroups.First();

            var spaces = FakeDataFactory.GenerateUserSpaces(5, userData.UserData.Id, statusGroup.Id);

            _dbContext.UserSpaces.AddRange(spaces);
            _dbContext.SaveChanges();

            var id = spaces.Last().Id;

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.UserSpaces, s => s.Id == id);
        }

        [Fact]
        public async Task DeleteByIdAsync_InvalidId_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
