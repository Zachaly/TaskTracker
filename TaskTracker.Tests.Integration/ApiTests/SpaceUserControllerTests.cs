using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.SpaceUser;
using TaskTracker.Model.UserSpace.Request;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class SpaceUserControllerTests : ApiTest
    {
        const string Endpoint = "api/space-user";

        [Fact]
        public async Task GetAsync_ReturnsUserAndSpace()
        {
            var spaceId = (await AuthorizeAndCreateSpaceAsync()).SpaceId;

            var users = FakeDataFactory.GenerateUsers(5);

            _dbContext.Users.AddRange(users);

            _dbContext.SaveChanges();

            var spaceUsers = FakeDataFactory.GenerateSpaceUsers(spaceId, users.Select(x => x.Id));

            _dbContext.SpaceUsers.AddRange(spaceUsers);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<SpaceUserModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.All(content.Select(x => x.User), Assert.NotNull);
            Assert.All(content.Select(x => x.Space), Assert.NotNull);
        }

        [Fact]
        public async Task PostAsync_ValidRequest_AddsNewSpaceUser()
        {
            var spaceId = (await AuthorizeAndCreateSpaceAsync()).SpaceId;

            var user = FakeDataFactory.GenerateUsers(1).First();

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var request = new AddSpaceUserCommand
            {
                SpaceId = spaceId,
                UserId = user.Id
            };

            _httpClient.DefaultRequestHeaders.Add("SpaceId", spaceId.ToString());
            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(_dbContext.SpaceUsers, u => u.UserId == request.UserId && u.SpaceId == request.SpaceId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsBadRequest()
        {
            var spaceId = (await AuthorizeAndCreateSpaceAsync()).SpaceId;

            var request = new AddSpaceUserCommand
            {
                SpaceId = 1,
                UserId = -1
            };

            _httpClient.DefaultRequestHeaders.Add("SpaceId", spaceId.ToString());
            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, key => key == "UserId");
        }

        [Fact]
        public async Task DeleteAsync_DeletesCorrectEntity()
        {
            var spaceId = (await AuthorizeAndCreateSpaceAsync()).SpaceId;

            var users = FakeDataFactory.GenerateUsers(5);

            _dbContext.Users.AddRange(users);

            _dbContext.SaveChanges();

            var spaceUsers = FakeDataFactory.GenerateSpaceUsers(spaceId, users.Select(x => x.Id));

            _dbContext.SpaceUsers.AddRange(spaceUsers);
            _dbContext.SaveChanges();

            var deletedUser = spaceUsers.First();

            _httpClient.DefaultRequestHeaders.Add("SpaceId", spaceId.ToString());
            var response = await _httpClient.DeleteAsync($"{Endpoint}/{deletedUser.SpaceId}/{deletedUser.UserId}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(_dbContext.SpaceUsers, u => u.UserId == deletedUser.UserId && u.SpaceId == deletedUser.SpaceId);
        }

        [Fact]
        public async Task DeleteAsync_SpaceUserNotFound_ReturnsBadRequest()
        {
            var spaceId = (await AuthorizeAndCreateSpaceAsync()).SpaceId;

            _httpClient.DefaultRequestHeaders.Add("SpaceId", spaceId.ToString());
            var response = await _httpClient.DeleteAsync($"{Endpoint}/{spaceId}/37");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
