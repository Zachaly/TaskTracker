using System.Net;
using System.Net.Http.Json;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTaskStatus;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class UserTaskStatusControllerTests : ApiTest
    {
        const string Endpoint = "api/user-task-status";

        [Fact]
        public async Task GetAsync_ReturnsStatuses()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var groups = FakeDataFactory.GenerateTaskStatusGroups(3, user.Id);

            _dbContext.TaskStatusGroups.AddRange(groups);
            _dbContext.SaveChanges();

            var statuses = groups.Select(x => x.Id).SelectMany(id => FakeDataFactory.GenerateTaskStatuses(5, id));

            _dbContext.UserTaskStatuses.AddRange(statuses);
            _dbContext.SaveChanges();

            var groupId = groups.Last().Id;

            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}?GroupId={groupId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserTaskStatusModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(_dbContext.UserTaskStatuses.Where(x => x.GroupId == groupId).Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProperTaskStatus()
        {
            await AuthorizeAsync();

            var status = _dbContext.UserTaskStatuses.First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{status.Id}");
            var content = await response.Content.ReadFromJsonAsync<UserTaskStatusModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(status.Name, content.Name);
            Assert.Equal(status.Index, content.Index);
            Assert.Equal(status.IsDefault, content.IsDefault);
            Assert.Equal(status.Color, content.Color);
        }

        [Fact]
        public async Task GetByIdAsync_StatusDoesNotExist_NotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_AddsNewStatus()
        {
            await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            var request = new AddUserTaskStatusRequest
            {
                Color = "col",
                GroupId = group.Id,
                Index = 1,
                Name = "Test"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.UserTaskStatuses, s => s.Id == content.NewEntityId &&
                s.Index == request.Index &&
                s.GroupId == request.GroupId &&
                s.Name == request.Name &&
                s.Color == request.Color &&
                !s.IsDefault);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_BadRequest()
        {
            await AuthorizeAsync();

            await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            var request = new AddUserTaskStatusRequest
            {
                Color = "",
                GroupId = group.Id,
                Index = 1,
                Name = "Test"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Color");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCorrectStatus()
        {
            await AuthorizeAsync();

            var status = _dbContext.UserTaskStatuses.First();

            var request = new UpdateUserTaskStatusRequest
            {
                Id = status.Id,
                Name = "new name",
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            await _dbContext.Entry(status).ReloadAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Name, status.Name);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_BadRequest()
        {
            await AuthorizeAsync();

            var status = _dbContext.UserTaskStatuses.First();

            var request = new UpdateUserTaskStatusRequest
            {
                Id = status.Id,
                Name = "",
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
        }

        [Fact]
        public async Task UpdateAsync_EntityNotFound_BadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateUserTaskStatusRequest
            {
                Id = 1237,
                Name = "",
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteByIdAsync_StatusDeleted()
        {
            await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            var status = FakeDataFactory.GenerateTaskStatuses(1, group.Id).First();

            _dbContext.UserTaskStatuses.Add(status);
            _dbContext.SaveChanges();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{status.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.UserTaskStatuses, s => s.Id == status.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_DefaultStatus_BadRequest()
        {
            await AuthorizeAsync();

            var status = _dbContext.UserTaskStatuses.First(x => x.IsDefault);

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{status.Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteByIdAsync_StatusDoesNotExist_BadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
