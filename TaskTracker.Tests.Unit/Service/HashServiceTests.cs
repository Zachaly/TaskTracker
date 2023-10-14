using Microsoft.Extensions.Configuration;
using NSubstitute;
using TaskTracker.Application.Authorization.Service;

namespace TaskTracker.Tests.Unit.Service
{
    public class HashServiceTests
    {
        private readonly HashService _service;

        public HashServiceTests()
        {
            var configuration = Substitute.For<IConfiguration>();
            configuration["HashKey"].Returns(new string('a', 24));

            _service = new HashService(configuration);
        }

        [Fact]
        public async Task HashStringAsync_CreatesHash()
        {
            var str = "string";

            var hash = await _service.HashStringAsync(str);

            Assert.NotEqual(str, hash);
        }

        [Fact]
        public async Task CompareStringWithHashAsync_ReturnsTrueForHashedString()
        {
            var str = "string";

            var hash = await _service.HashStringAsync(str);

            Assert.True(await _service.CompareStringWithHashAsync(str, hash));
        }

        [Fact]
        public async Task CompareStringWithHashAsync_ReturnsFalseForWrongString()
        {
            var str = "string";

            var hash = await _service.HashStringAsync(str);

            Assert.False(await _service.CompareStringWithHashAsync("anotherstring", hash));
        }
    }
}
