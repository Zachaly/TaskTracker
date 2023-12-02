using System.Net;
using TaskTracker.Model.TaskStatusGroup;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Model.TaskStatusGroup.Request;
using TaskTracker.Model.Response;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class TaskStatusGroupControllerTests : ApiTest
    {
        const string Endpoint = "api/task-status-group";

        [Fact]
        public async Task GetAsync_ReturnsStatusGroups()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var groups = FakeDataFactory.GenerateTaskStatusGroups(5, user.Id);

            _dbContext.TaskStatusGroups.AddRange(groups);
            _dbContext.SaveChanges();

            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}?UserId={user.Id}");

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<TaskStatusGroupModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(groups.Select(x => x.Id), content.Select(x => x.Id));
            Assert.All(groups, g => Assert.Null(g.Statuses));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectGroupWithStatuses()
        {
            await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.Include(x => x.Statuses).First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{group.Id}");
            var content = await response.Content.ReadFromJsonAsync<TaskStatusGroupModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(group.Id, content.Id);
            Assert.Equal(group.Name, content.Name);
            Assert.Equal(group.IsDefault, content.IsDefault);
            Assert.Equivalent(group.Statuses.Select(x => x.Id), content.Statuses.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_GroupDoesNotExists_NotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_AddsNewStatusGroup_WithDefaultStatuses()
        {
            var loginData = await AuthorizeAsync();

            var request = new AddTaskStatusGroupRequest
            {
                Name = "fun name",
                UserId = loginData.UserData!.Id
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.TaskStatusGroups, g => g.Id == content.NewEntityId &&
                g.Name == request.Name &&
                g.UserId == request.UserId);
            Assert.Equal(2, _dbContext.UserTaskStatuses.Where(s => s.GroupId == content.NewEntityId && s.IsDefault).Count());
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_BadRequest()
        {
            var loginData = await AuthorizeAsync();

            var request = new AddTaskStatusGroupRequest
            {
                Name = "",
                UserId = loginData.UserData!.Id
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCorrectStatusGroup()
        {
            await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            var request = new UpdateTaskStatusGroupRequest
            {
                Id = group.Id,
                Name = "new name"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            await _dbContext.Entry(group).ReloadAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Name, group.Name);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_BadRequest()
        {
            await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            var request = new UpdateTaskStatusGroupRequest
            {
                Id = group.Id,
                Name = ""
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

            var request = new UpdateTaskStatusGroupRequest
            {
                Id = 2137,
                Name = "new name"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesCorrectEntity()
        {
            var loginData = await AuthorizeAsync();

            var group = FakeDataFactory.GenerateTaskStatusGroups(1, loginData.UserData!.Id).First();

            _dbContext.TaskStatusGroups.Add(group);
            _dbContext.SaveChanges();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{group.Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.TaskStatusGroups, g => g.Id == group.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_DefaultGroup_BadRequest()
        {
            await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{group.Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteByIdAsync_InvalidId_BadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
