using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddUserSpaceCommandValidatorTests
    {
        private readonly AddUserSpaceCommandValidator _validator;

        public AddUserSpaceCommandValidatorTests()
        {
            _validator = new AddUserSpaceCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddUserSpaceCommand
            {
                UserId = 1,
                StatusGroupId = 2,
                Title = "Test",
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddUserSpaceCommand
            {
                UserId = -1,
                StatusGroupId = 2,
                Title = "Test",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidStatusGroupId_DoesNotPassValidation()
        {
            var request = new AddUserSpaceCommand
            {
                UserId = 1,
                StatusGroupId = -2,
                Title = "Test",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidTitleLength_DoesNotPassValidation(int length)
        {
            var request = new AddUserSpaceCommand
            {
                UserId = 1,
                StatusGroupId = 2,
                Title = new string('a', length),
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
