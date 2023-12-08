using System.Net;
using System.Net.Http.Json;
using TaskTracker.Application.Authorization.Command;
using TaskTracker.Application.Command;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.User;
using TaskTracker.Model.User.Request;

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

            var userId = (await registerResponse.Content.ReadFromJsonAsync<CreatedResponseModel>())!.NewEntityId!;

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
            Assert.Contains(_dbContext.RefreshTokens, t => t.UserId == userId && t.Token == content.RefreshToken);
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

        [Fact]
        public async Task RefreshToken_ReturnsNewTokens()
        {
            var loginData = await AuthorizeAsync();

            var request = new RefreshTokenCommand
            {
                AccessToken = loginData.AccessToken!,
                RefreshToken = loginData.RefreshToken!
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/refresh-token", request);

            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(content.AccessToken);
            Assert.NotEmpty(content.RefreshToken);
            Assert.NotEqual(loginData.RefreshToken, content.RefreshToken);
            Assert.NotEqual(loginData.AccessToken, content.AccessToken);
            Assert.False(_dbContext.Set<RefreshToken>().First(x => x.Token == loginData.RefreshToken).IsValid);
            Assert.True(_dbContext.Set<RefreshToken>().First(x => x.Token == content.RefreshToken).IsValid);
        }

        [Fact]
        public async Task RefreshToken_InvalidRefreshToken_BadRequest()
        {
            var loginData = await AuthorizeAsync();

            var request = new RefreshTokenCommand
            {
                AccessToken = loginData.AccessToken!,
                RefreshToken = "invalid token"
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/refresh-token", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidatedRefreshToken_BadRequest()
        {
            var loginData = await AuthorizeAsync();

            var token = _dbContext.Set<RefreshToken>().First(x => x.Token == loginData.RefreshToken);
            token.IsValid = false;
            _dbContext.Set<RefreshToken>().Update(token);
            _dbContext.SaveChanges();

            var request = new RefreshTokenCommand
            {
                AccessToken = loginData.AccessToken!,
                RefreshToken = loginData.RefreshToken!
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/refresh-token", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidAccessToken_BadRequest()
        {
            var loginData = await AuthorizeAsync();

            var request = new RefreshTokenCommand
            {
                AccessToken = "not jwt token",
                RefreshToken = loginData.RefreshToken!
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/refresh-token", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RevokeToken_RevokesRefreshToken()
        {
            var loginData = await AuthorizeAsync();

            var request = new InvalidateRefreshTokenCommand
            {
                RefreshToken = loginData.RefreshToken!
            };

            var response = await _httpClient.PutAsJsonAsync($"{Endpoint}/revoke-token", request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.False(_dbContext.RefreshTokens.First(x => x.Token == request.RefreshToken).IsValid);
        }

        [Fact]
        public async Task ChangeUserPassword_PasswordChanged()
        {
            var loginData = await AuthorizeAsync();

            var request = new ChangeUserPasswordRequest
            {
                UserId = loginData.UserData!.Id,
                CurrentPassword = "password",
                NewPassword = "newPassword"
            };

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/change-password", request);

            var loginRequest = new LoginCommand
            {
                Email = loginData.UserData.Email,
                Password = request.NewPassword
            };

            var loginResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        }

        [Fact]
        public async Task ChangeUserPassword_UserNotFound_BadRequest()
        {
            await AuthorizeAsync();

            var request = new ChangeUserPasswordRequest
            {
                UserId = 2137,
                CurrentPassword = "password",
                NewPassword = "newPassword"
            };

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/change-password", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ChangeUserPassword_InvalidCurrentPassword_BadRequest()
        {
            var loginData = await AuthorizeAsync();

            var request = new ChangeUserPasswordRequest
            {
                UserId = loginData.UserData!.Id,
                CurrentPassword = "pass",
                NewPassword = "newPassword"
            };

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/change-password", request);

            var loginRequest = new LoginCommand
            {
                Email = loginData.UserData.Email,
                Password = request.NewPassword
            };

            var loginResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
        }

        [Fact]
        public async Task ChangeUserPasswordCommand_InvalidRequest_BadRequest()
        {
            var loginData = await AuthorizeAsync();

            var request = new ChangeUserPasswordRequest
            {
                UserId = loginData.UserData!.Id,
                CurrentPassword = "password",
                NewPassword = ""
            };

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/change-password", request);
            var content = await GetContentFromBadRequest<ResponseModel>(response);

            var loginRequest = new LoginCommand
            {
                Email = loginData.UserData.Email,
                Password = request.NewPassword
            };

            var loginResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "NewPassword");
        }
    }
}
