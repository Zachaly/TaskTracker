using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Authorization.Command;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.User;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class AuthorizationControllerTests : ApiTest
    {
        const string Endpoint = "api/auth";

        [Fact]
        public async Task Login_ReturnsTokens()
        {
            var registerRequest = new RegisterCommand
            {
                Email = "email@email.com",
                Password = "password",
                FirstName = "fname",
                LastName = "lname"
            };

            var registerResponse = await _httpClient.PostAsJsonAsync("api/user", registerRequest);

            var userId = (await registerResponse.Content.ReadFromJsonAsync<CreatedResponseModel>()).NewEntityId!;

            var loginRequest = new LoginCommand
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);

            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(content.AccessToken);
            Assert.NotEmpty(content.RefreshToken);
            Assert.Equal(registerRequest.FirstName, content.UserData.FirstName);
            Assert.Equal(registerRequest.LastName, content.UserData.LastName);
            Assert.Equal(registerRequest.Email, content.UserData.Email);
            Assert.Equal(userId, content.UserData.Id);
            Assert.Contains(_dbContext.Users, x => x.Id == userId && x.RefreshToken == content.RefreshToken);
        }

        [Fact]
        public async Task Login_InvalidPassword_BadRequest()
        {
            var registerRequest = new RegisterCommand
            {
                Email = "email@email.com",
                Password = "password",
                FirstName = "fname",
                LastName = "lname"
            };

            var loginRequest = new LoginCommand
            {
                Email = registerRequest.Email,
                Password = "invalidpassword",
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);

            var content = await GetContentFromBadRequest<DataResponseModel<LoginResponse>>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }

        [Fact]
        public async Task Login_UserDoesNotExists_BadRequest()
        {
            var request = new LoginCommand
            {
                Email = "email",
                Password = "pass"
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", request);

            var content = await GetContentFromBadRequest<DataResponseModel<LoginResponse>>(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
        }
    }
}
