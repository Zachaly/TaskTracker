using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class UpdateUserTaskCommandValidatorTests
    {
        private readonly UpdateUserTaskCommandValidator _validator;

        public UpdateUserTaskCommandValidatorTests()
        {
            _validator = new UpdateUserTaskCommandValidator();
        }

        [Fact]
        public async Task ValidRequest_OnlyRequiredFields_PassesValidation()
        {
            var request = new UpdateUserTaskCommand
            {
                Id = 1
            };

            var result = await _validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ValidRequest_PassesValidation()
        {
            var request = new UpdateUserTaskCommand
            {
                Id = 1,
                Title = "title",
                Description = "desc",
                DueTimestamp = 2137
            };

            var result = await _validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateUserTaskCommand
            {
                Id = -1,
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task InvalidDescriptionLength_DoesNotPassValidation()
        {
            var request = new UpdateUserTaskCommand
            {
                Id = 1,
                Description = new string('a', 1001),
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task InvalidTitleLength_DoesNotPassValidation()
        {
            var request = new UpdateUserTaskCommand
            {
                Id = 1,
                Title = new string('a', 201),
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task InvalidDueTimestamp_DoesNotPassValidation()
        {
            var request = new UpdateUserTaskCommand
            {
                Id = 1,
                DueTimestamp = -1
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }
    }
}
