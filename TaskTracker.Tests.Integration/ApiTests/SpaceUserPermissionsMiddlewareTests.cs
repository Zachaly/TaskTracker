using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class SpaceUserPermissionsMiddlewareTests : ApiTest
    {
        private long CreateSpace()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var statusGroup = FakeDataFactory.GenerateTaskStatusGroups(1, user.Id).First();

            _dbContext.TaskStatusGroups.Add(statusGroup);
            _dbContext.SaveChanges();

            var space = FakeDataFactory.GenerateUserSpaces(1, user.Id, statusGroup.Id).First();
            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            return space.Id;
        }

        [Fact]
        public async Task SpaceIdHeaderNotSpecified_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            var response = await _httpClient.PostAsJsonAsync("/api/space-user", new { });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task InvalidSpaceIdHeader_ReturnsBadRequest()
        {
            await AuthorizeAsync();

            _httpClient.DefaultRequestHeaders.Add("SpaceId", "not a number");

            var response = await _httpClient.PostAsJsonAsync("/api/space-user", new { });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SpaceIdHeaderSpecified_SpaceNotFound_ReturnsNotFound()
        {
            await AuthorizeAsync();

            _httpClient.DefaultRequestHeaders.Add("SpaceId", "1");

            var response = await _httpClient.PostAsJsonAsync("/api/space-user", new { });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SpaceIdHeaderSpecified_UserWithNoPermissions_ReturnsForbidden()
        {
            await AuthorizeAsync();

            var spaceId = CreateSpace();

            _httpClient.DefaultRequestHeaders.Add("SpaceId", spaceId.ToString());

            var response = await _httpClient.PostAsJsonAsync("/api/space-user", new { });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task SpaceIdHeaderSpecified_UserHasRequiredPermission_ReturnsSuccessStatusCode()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var spaceId = CreateSpace();

            var permissions = new SpaceUserPermissions
            {
                UserId = userId,
                SpaceId = spaceId,
                CanAddUsers = true
            };

            _dbContext.SpaceUserPermissions.Add(permissions);
            _dbContext.SaveChanges();

            _httpClient.DefaultRequestHeaders.Add("SpaceId", spaceId.ToString());
            var response = await _httpClient.PostAsJsonAsync("/api/space-user", new AddSpaceUserCommand 
            { 
                SpaceId = spaceId,
                UserId = userId
            });

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task SpaceIdHeaderSpecified_UserDoesNotHaveRequiredPermission_ReturnsForbidden()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var spaceId = CreateSpace();

            var permissions = new SpaceUserPermissions
            {
                UserId = userId,
                SpaceId = spaceId,
                CanAddUsers = false
            };

            _dbContext.SpaceUserPermissions.Add(permissions);
            _dbContext.SaveChanges();

            _httpClient.DefaultRequestHeaders.Add("SpaceId", spaceId.ToString());
            var response = await _httpClient.PostAsJsonAsync("/api/space-user", new AddSpaceUserCommand
            {
                SpaceId = spaceId,
                UserId = userId
            });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
