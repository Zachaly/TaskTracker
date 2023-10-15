using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
    }
}
