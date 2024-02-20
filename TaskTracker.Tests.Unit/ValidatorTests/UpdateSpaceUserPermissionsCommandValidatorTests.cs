using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class UpdateSpaceUserPermissionsCommandValidatorTests
    {
        private readonly UpdateSpaceUserPermissionsCommandValidator _validator;

        public UpdateSpaceUserPermissionsCommandValidatorTests()
        {
            _validator = new UpdateSpaceUserPermissionsCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateSpaceUserPermissionsCommand
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
            var request = new UpdateSpaceUserPermissionsCommand
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
            var request = new UpdateSpaceUserPermissionsCommand
            {
                UserId = 1,
                SpaceId = 0,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
