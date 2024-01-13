using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.User;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class UserControllerTests : ApiTest
    {
        private const string Endpoint = "api/user";

        [Fact]
        public async Task Register_CreatesNewUser()
        {
            var request = new RegisterCommand
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password",
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await response.Content.ReadFromJsonAsync<CreatedResponseModel>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(_dbContext.Users.AsEnumerable(), x => x.Email == request.Email 
                && x.FirstName == request.FirstName
                && x.LastName == request.LastName
                && x.Id == content.NewEntityId);
            Assert.Contains(_dbContext.TaskStatusGroups.AsEnumerable(), x => x.UserId == content.NewEntityId && x.IsDefault);
        }

        [Fact]
        public async Task Register_InvalidRequest_BadRequest()
        {
            var request = new RegisterCommand
            {
                Email = "email",
                LastName = "lname",
                Password = "password",
                FirstName = "fname"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await GetContentFromBadRequest<CreatedResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.ValidationErrors["Email"]);
        }

        [Fact]
        public async Task Register_EmailTaken_BadRequest()
        {
            const string Email = "email@email.com";

            _dbContext.Set<User>().Add(new User
            {
                Email = Email,
                PasswordHash = "hash",
                FirstName = "fname",
                LastName = "lname",
            });

            _dbContext.SaveChanges();

            var request = new RegisterCommand
            {
                Email = Email,
                FirstName = "fname2",
                LastName = "lname2",
                Password = "pass"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequest<CreatedResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesUser()
        {
            var loginData = await AuthorizeAsync();

            var request = new UpdateUserRequest
            {
                Id = loginData.UserData!.Id,
                FirstName = "new fname",
                LastName = "new lname"
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(_dbContext.Users, x => x.Id == request.Id 
                && x.FirstName == request.FirstName 
                && x.LastName == request.LastName);
        }

        [Fact]
        public async Task UpdateAsync_UserNotFound_BadRequest()
        {
            await AuthorizeAsync();

            var request = new UpdateUserRequest
            {
                Id = 2137,
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_BadRequest()
        {
            var loginData = await AuthorizeAsync();

            var request = new UpdateUserRequest
            {
                Id = loginData.UserData!.Id,
                LastName = ""
            };

            var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
            var content = await GetContentFromBadRequest<ResponseModel>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "LastName");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnCorrectUser()
        {
            var userData = (await AuthorizeAsync()).UserData!;

            var response = await _httpClient.GetAsync($"{Endpoint}/{userData.Id}");
            var content = await response.Content.ReadFromJsonAsync<UserModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(userData.FirstName, content.FirstName);
            Assert.Equal(userData.LastName, content.LastName);
            Assert.Equal(userData.Id, content.Id);
            Assert.Equal(userData.Email, content.Email);
        }

        [Fact]
        public async Task GetByIdAsync_UserNotFound_Failure()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]    
        public async Task GetAsync_ReturnsUsers()
        {
            await AuthorizeAsync();

            var users = FakeDataFactory.GenerateUsers(5);

            _dbContext.Users.AddRange(users);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(_dbContext.Users.Select(x => x.Id), content.Select(x => x.Id));
        }
    }
}
