using System.Net;
using System.Net.Http.Json;
using TaskTracker.Model.SpaceUserPermissions;
using TaskTracker.Model.SpaceUserPermissions.Request;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class SpaceUserPermissionsControllerTests : ApiTest
    {
        const string Endpoint = "/api/space-user-permissions";

        [Fact]
        public async Task GetAsync_ReturnsProperEntities()
        {
            var userData = await AuthorizeAsync();

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userData.UserData!.Id!, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var users = FakeDataFactory.GenerateUsers(5);

            _dbContext.Users.AddRange(users);

            _dbContext.SaveChanges();

            var permissions = FakeDataFactory.GenerateSpaceUserPermissions(space.Id, users.Select(x => x.Id));

            _dbContext.SpaceUserPermissions.AddRange(permissions);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<SpaceUserPermissionsModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(permissions.Select(x => x.UserId), content.Select(x => x.User.Id));
            Assert.Equivalent(permissions.Select(x => x.SpaceId), content.Select(x => x.SpaceId));
        }

        [Fact]
        public async Task PostAsync_ValidRequest_AddsNewEntity()
        {
            var userData = await AuthorizeAsync();

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userData.UserData!.Id!, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var users = FakeDataFactory.GenerateUsers(1);

            _dbContext.Users.AddRange(users);

            _dbContext.SaveChanges();

            var userId = users.First().Id;

            var request = new AddSpaceUserPermissionsRequest
            {
                UserId = userId,
                SpaceId = space.Id
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(_dbContext.SpaceUserPermissions, p => p.UserId == request.UserId && p.SpaceId == request.SpaceId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            var userData = await AuthorizeAsync();

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userData.UserData!.Id!, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var request = new AddSpaceUserPermissionsRequest
            {
                UserId = 0,
                SpaceId = space.Id
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync_ValidRequest_UpdatesProperEntity()
        {
            var userData = await AuthorizeAsync();

            var statusGroupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userData.UserData!.Id!, statusGroupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var users = FakeDataFactory.GenerateUsers(1);

            _dbContext.Users.AddRange(users);

            _dbContext.SaveChanges();

            var permissions = FakeDataFactory.GenerateSpaceUserPermissions(space.Id, users.Select(x => x.Id)).First();

            _dbContext.SpaceUserPermissions.Add(permissions);
            _dbContext.SaveChanges();

            var request = new UpdateSpaceUserPermissionsRequest
            {
                UserId = permissions.UserId,
                SpaceId = permissions.SpaceId,
                CanAddUsers = true
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            await _dbContext.Entry(permissions).ReloadAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.CanAddUsers, permissions.CanAddUsers);
        }

        [Fact]
        public async Task PutAsync_InvalidRequest_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateSpaceUserPermissionsRequest
            {
                UserId = -1,
                SpaceId = 1,
                CanAddUsers = true
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync_EntityNotFound_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateSpaceUserPermissionsRequest
            {
                UserId = 1,
                SpaceId = 1,
                CanAddUsers = true
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
