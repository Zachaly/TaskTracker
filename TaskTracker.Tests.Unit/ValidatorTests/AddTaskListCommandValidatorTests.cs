using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddTaskListCommandValidatorTests
    {
        private readonly AddTaskListCommandValidator _validator;

        public AddTaskListCommandValidatorTests()
        {
            _validator = new AddTaskListCommandValidator();
        }

        [Fact]
        public async Task ValidRequest_PassesValidation()
        {
            var request = new AddTaskListCommand
            {
                CreatorId = 1,
                Title = "Test",
                SpaceId = 1,
            };

            var res = await _validator.ValidateAsync(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public async Task InvalidCreatorId_DoesNotPassValidation()
        {
            var request = new AddTaskListCommand
            {
                CreatorId = 0,
                Title = "title",
                SpaceId = 2
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(201)]
        public async Task InvalidTitleLength_DoesNotPassValidation(int length)
        {
            var request = new AddTaskListCommand
            {
                CreatorId = 1,
                Title = new string('a', length),
                SpaceId = 2
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public async Task InvalidColorLength_DoesNotPassValidation()
        {
            var request = new AddTaskListCommand
            {
                CreatorId = 1,
                Title = "title",
                Color = new string('a', 11),
                SpaceId = 2
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public async Task InvalidDescriptionLength_DoesNotPassValidation()
        {
            var request = new AddTaskListCommand
            {
                CreatorId = 1,
                Title = "title",
                Description = new string('a', 1001),
                SpaceId = 2
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public async Task InvalidSpaceId_DoesNotPassValidation()
        {
            var request = new AddTaskListCommand
            {
                CreatorId = 1,
                Title = "title",
                Description = "desc",
                SpaceId = -1
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }
    }
}
