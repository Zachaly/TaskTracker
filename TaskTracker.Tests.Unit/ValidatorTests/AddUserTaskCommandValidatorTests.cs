using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddUserTaskCommandValidatorTests
    {
        private readonly AddUserTaskCommandValidator _validator;

        public AddUserTaskCommandValidatorTests()
        {
            _validator = new AddUserTaskCommandValidator();
        }

        [Fact]
        public async Task ValidRequest_PassesValidation()
        {
            var request = new AddUserTaskCommand
            {
                CreatorId = 1,
                Description = "Test",
                Title = "Test",
            };

            var result = await _validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task InvalidCreatorId_DoesNotPassValidation()
        {
            var request = new AddUserTaskCommand
            {
                CreatorId = -1,
                Description = "Test",
                Title = "Test",
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task InvalidDescriptionLength_DoesNotPassValidation()
        {
            var request = new AddUserTaskCommand
            {
                CreatorId = 1,
                Description = new string('a', 1001),
                Title = "Test",
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(201)]
        public async Task InvalidTitleLength_DoesNotPassValidation(int length)
        {
            var request = new AddUserTaskCommand
            {
                CreatorId = -1,
                Description = "Test",
                Title = new string('a', length),
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task InvalidDueTimestamp_DoesNotPassValidation()
        {
            var request = new AddUserTaskCommand
            {
                CreatorId = 0,
                Description = "Test",
                Title = "Test",
                DueTimestamp = -1
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }
    }
}
