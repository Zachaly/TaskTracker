using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class TaskListControllerTests : ApiTest
    {
        const string Endpoint = "api/task-list";

        [Fact]
        public async Task PostAsync_CreatesNewList()
        {
            var userData = await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            var request = new AddTaskListCommand
            {
                CreatorId = userData.UserData!.Id,
                Description = "desc",
                Color = "#000000",
                Title = "title",
                StatusGroupId = group.Id,
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            var id = content!.NewEntityId!;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.TaskLists, list => 
                list.Id == id &&
                list.CreatorId == request.CreatorId &&
                list.Title == request.Title &&
                list.Description == request.Description &&
                list.TaskStatusGroupId == request.StatusGroupId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            var userData = await AuthorizeAsync();

            var request = new AddTaskListCommand
            {
                CreatorId = userData.UserData!.Id,
                Description = "desc",
                Color = "#000000",
                Title = new string('a', 201),
                StatusGroupId = 1
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<CreatedResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
            Assert.Contains(content.ValidationErrors.Keys, key => key == "Title");
        }

        [Fact]
        public async Task GetAsync_ReturnsCorrectLists()
        {
            var userData = await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            _dbContext.TaskLists.AddRange(FakeDataFactory.GenerateTaskLists(5, userData.UserData!.Id, group.Id));
            await _dbContext.SaveChangesAsync();

            var user = FakeDataFactory.GenerateUsers(1).First();
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            _dbContext.TaskLists.AddRange(FakeDataFactory.GenerateTaskLists(5, user.Id, group.Id));
            await _dbContext.SaveChangesAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}?CreatorId={userData.UserData.Id}");

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<TaskListModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(_dbContext.TaskLists.Where(x => x.CreatorId == userData.UserData.Id).Select(x => x.Id),
                content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSpecifiedList()
        {
            var userData = await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            _dbContext.TaskLists.AddRange(FakeDataFactory.GenerateTaskLists(5, userData.UserData!.Id, group.Id));
            await _dbContext.SaveChangesAsync();

            var list = _dbContext.TaskLists.OrderBy(x => x.Id).Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{list.Id}");

            var content = await response.Content.ReadFromJsonAsync<TaskListModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(list.Id, content.Id);
            Assert.Equal(list.Title, content.Title);
            Assert.Equal(list.Description, content.Description);
            Assert.Equal(userData.UserData.Id, content.Creator.Id);
            Assert.Equal(userData.UserData.FirstName, content.Creator.FirstName);
            Assert.Equal(userData.UserData.LastName, content.Creator.LastName);
            Assert.Equal(userData.UserData.Email, content.Creator.Email);
        }

        [Fact]
        public async Task GetByIdAsync_NoListWithSpecifiedId_ReturnsNotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync_UpdatesProperList()
        {
            var userData = await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            _dbContext.TaskLists.AddRange(FakeDataFactory.GenerateTaskLists(2, userData.UserData!.Id, group.Id));
            await _dbContext.SaveChangesAsync();

            var list = _dbContext.TaskLists.AsNoTracking().First();

            var request = new UpdateTaskListRequest
            {
                Id = list.Id,
                Description = "new desc",
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var updatedList = _dbContext.TaskLists.First(x => x.Id == request.Id);

            await _dbContext.Entry(updatedList).ReloadAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(list.Title, updatedList.Title);
            Assert.Equal(list.Color, updatedList.Color);
            Assert.Equal(request.Description, updatedList.Description);
        }

        [Fact]
        public async Task PutAsync_ListNotFound_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateTaskListRequest
            {
                Id = 2137
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync_InvalidRequest_ReturnsBadRequest()
        {
            var userData = await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            _dbContext.TaskLists.AddRange(FakeDataFactory.GenerateTaskLists(1, userData.UserData!.Id, group.Id));
            await _dbContext.SaveChangesAsync();

            var list = _dbContext.TaskLists.First();

            var request = new UpdateTaskListRequest
            {
                Id = list.Id,
                Title = ""
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Title");
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesListWithTasks()
        {
            var userData = await AuthorizeAsync();

            var group = _dbContext.TaskStatusGroups.First();

            _dbContext.TaskLists.AddRange(FakeDataFactory.GenerateTaskLists(5, userData.UserData!.Id, group.Id));
            await _dbContext.SaveChangesAsync();

            var listIds = _dbContext.TaskLists.Select(x => x.Id).ToList();

            var status = _dbContext.UserTaskStatuses.First();

            _dbContext.Tasks.AddRange(listIds.SelectMany(id => FakeDataFactory.GenerateUserTasks(3, userData.UserData.Id, id, status.Id)));
            await _dbContext.SaveChangesAsync();

            var deletedListId = listIds.Last();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{deletedListId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.TaskLists, x => x.Id == deletedListId);
            Assert.DoesNotContain(_dbContext.Tasks, x => x.ListId == deletedListId);
        }

        [Fact]
        public async Task DeleteByIdAsync_NoListWithSpecifiedId_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
