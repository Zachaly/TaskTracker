using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Command;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;

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
    }
}
