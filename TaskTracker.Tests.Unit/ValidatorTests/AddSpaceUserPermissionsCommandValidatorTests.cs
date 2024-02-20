using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddSpaceUserPermissionsCommandValidatorTests
    {
        private readonly AddSpaceUserPermissionsCommandValidator _validator;

        public AddSpaceUserPermissionsCommandValidatorTests()
        {
            _validator = new AddSpaceUserPermissionsCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddSpaceUserPermissionsCommand
            {
                UserId = 1,
                SpaceId = 2,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddSpaceUserPermissionsCommand
            {
                UserId = 0,
                SpaceId = 2,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidSpaceId_DoesNotPassValidation()
        {
            var request = new AddSpaceUserPermissionsCommand
            {
                UserId = 1,
                SpaceId = 0,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
