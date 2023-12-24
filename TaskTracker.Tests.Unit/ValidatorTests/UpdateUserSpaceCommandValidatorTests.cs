using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class UpdateUserSpaceCommandValidatorTests
    {
        private readonly UpdateUserSpaceCommandValidator _validator;

        public UpdateUserSpaceCommandValidatorTests()
        {
            _validator = new UpdateUserSpaceCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateUserSpaceCommand
            {
                Id = 1,
                Title = "Test",
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ValidRequest_OnlyRequiredFields_PassesValidation()
        {
            var request = new UpdateUserSpaceCommand { Id = 1, };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateUserSpaceCommand()
            {
                Id = -1,
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidTitleLength_DoesNotPassValidation(int length)
        {
            var request = new UpdateUserSpaceCommand
            {
                Id = 1,
                Title = new string('a', length)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
