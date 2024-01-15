using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskAssignedUser;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class TaskAssignedUserControllerTests : ApiTest
    {
        const string Endpoint = "/api/task-assigned-user";

        [Fact]
        public async Task GetAsync_ReturnsUsers()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var taskList = FakeDataFactory.GenerateTaskLists(1, userId, statusGroupId, space.Id).First();

            _dbContext.TaskLists.Add(taskList);
            _dbContext.SaveChanges();

            var task = FakeDataFactory.GenerateUserTasks(1, userId, taskList.Id, _dbContext.UserTaskStatuses.First().Id).First();

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            var users = FakeDataFactory.GenerateUsers(5);

            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            var assignedUsers = FakeDataFactory.GenerateTaskAssignedUsers(task.Id, users.Select(x => x.Id));

            _dbContext.TaskAssignedUsers.AddRange(assignedUsers);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<TaskAssignedUserModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(assignedUsers.Select(x => x.TaskId), content.Select(x => x.Task.Id));
            Assert.Equivalent(assignedUsers.Select(x => x.UserId), content.Select(x => x.User.Id));
        }

        [Fact]
        public async Task PostAsync_ValidRequest_AddsNewAssignedUser()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var taskList = FakeDataFactory.GenerateTaskLists(1, userId, statusGroupId, space.Id).First();

            _dbContext.TaskLists.Add(taskList);
            _dbContext.SaveChanges();

            var task = FakeDataFactory.GenerateUserTasks(1, userId, taskList.Id, _dbContext.UserTaskStatuses.First().Id).First();

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            var request = new AddTaskAssignedUserCommand
            {
                UserId = userId,
                TaskId = task.Id
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(_dbContext.TaskAssignedUsers, user => user.TaskId == request.TaskId && user.UserId == request.UserId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new AddTaskAssignedUserCommand
            {
                UserId = -1,
                TaskId = 1
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, k => k == "UserId");
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperAssignedUser()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var taskList = FakeDataFactory.GenerateTaskLists(1, userId, statusGroupId, space.Id).First();

            _dbContext.TaskLists.Add(taskList);
            _dbContext.SaveChanges();

            var task = FakeDataFactory.GenerateUserTasks(1, userId, taskList.Id, _dbContext.UserTaskStatuses.First().Id).First();

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            var users = FakeDataFactory.GenerateUsers(5);

            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            var assignedUsers = FakeDataFactory.GenerateTaskAssignedUsers(task.Id, users.Select(x => x.Id));

            _dbContext.TaskAssignedUsers.AddRange(assignedUsers);
            _dbContext.SaveChanges();

            var deletedUser = assignedUsers.Last();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{deletedUser.TaskId}/{deletedUser.UserId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.TaskAssignedUsers, user => user.TaskId == deletedUser.TaskId 
                && user.UserId == deletedUser.UserId);
        }

        [Fact]
        public async Task DeleteAsync_UserDoesNotExists_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/21/37");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
