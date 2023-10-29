using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class UserTaskControllerTests : ApiTest
    {
        const string Endpoint = "api/user-task";

        [Fact]
        public async Task GetAsync_ReturnsCorrectTasks()
        {
            _dbContext.Users.AddRange(FakeDataFactory.GenerateUsers(2));
            _dbContext.SaveChanges();

            var userIds = _dbContext.Users.Select(x => x.Id).ToList();
            var tasks = userIds.SelectMany(id => FakeDataFactory.GenerateUserTasks(3, id));

            _dbContext.Tasks.AddRange(tasks);
            _dbContext.SaveChanges();

            var userId = userIds.First();

            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}?UserId={userId}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserTaskModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(_dbContext.Tasks.Where(x => x.CreatorId == userId).Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectTask()
        {
            var loginData = await AuthorizeAsync();

            var task = new UserTask
            {
                CreatorId = loginData.UserData.Id,
                CreationTimestamp = 1,
                DueTimestamp = 2,
                Description = "desc",
                Title = "title"
            };

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync($"{Endpoint}/{task.Id}");
            var content = await response.Content.ReadFromJsonAsync<UserTaskModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(task.Id, content.Id);
            Assert.Equal(task.Title, content.Title);
            Assert.Equal(task.Description, content.Description);
            Assert.Equal(task.DueTimestamp, content.DueTimestamp);
            Assert.Equal(task.CreationTimestamp, content.CreationTimestamp);
            Assert.Equal(loginData.UserData.Email, content.Creator.Email);
            Assert.Equal(loginData.UserData.FirstName, content.Creator.FirstName);
            Assert.Equal(loginData.UserData.LastName, content.Creator.LastName);
            Assert.Equal(loginData.UserData.Id, content.Creator.Id);
        }

        [Fact]
        public async Task GetByIdAsync_NoTaskWithSpecifiedId_ReturnsNotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_AddsNewTask()
        {
            var logindata = await AuthorizeAsync();

            var request = new AddUserTaskCommand
            {
                CreatorId = logindata.UserData.Id,
                Description = "desc",
                DueTimestamp = 1,
                Title = "title"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.Tasks, t => t.DueTimestamp == request.DueTimestamp &&
                t.Title == request.Title &&
                t.Description == request.Description &&
                t.CreatorId == request.CreatorId &&
                t.Id == content.NewEntityId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            var logindata = await AuthorizeAsync();

            var request = new AddUserTaskCommand
            {
                CreatorId = logindata.UserData.Id,
                Description = "desc",
                DueTimestamp = 1,
                Title = ""
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequest<CreatedResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.ValidationErrors["Title"]);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesCorrectTask()
        {
            var loginData = await AuthorizeAsync();

            _dbContext.Tasks.AddRange(FakeDataFactory.GenerateUserTasks(5, loginData.UserData.Id));
            _dbContext.SaveChanges();

            var taskId = _dbContext.Tasks.OrderBy(t => t.Id).Last().Id;

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{taskId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.Tasks, x => x.Id == taskId);
        }

        [Fact]
        public async Task DeleteByIdAsync_TaskNotFound_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/2137");
            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }
    }
}
