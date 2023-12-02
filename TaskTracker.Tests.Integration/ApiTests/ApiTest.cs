using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskTracker.Application.Authorization.Command;
using TaskTracker.Application.Command;
using TaskTracker.Model.User;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class ApiTest : DatabaseTest
    {
        protected readonly HttpClient _httpClient;

        public ApiTest()
        {
            var webFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:SqlServer"] = Constants.ConnectionString,
                    });
                });
            });

            _httpClient = webFactory.CreateClient();
        }

        protected async Task<T?> GetContentFromBadRequest<T>(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContent);
        }

        protected async Task<LoginResponse> AuthorizeAsync()
        {
            var registerRequest = new RegisterCommand
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                Password = "password"
            };

            await _httpClient.PostAsJsonAsync("api/user", registerRequest);

            var loginRequest = new LoginCommand
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content!.AccessToken);

            return content;
        }
    }
}
