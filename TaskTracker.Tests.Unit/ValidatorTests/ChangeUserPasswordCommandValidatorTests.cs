using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class ChangeUserPasswordCommandValidatorTests
    {
        private readonly ChangeUserPasswordCommandValidator _validator;

        public ChangeUserPasswordCommandValidatorTests()
        {
            _validator = new ChangeUserPasswordCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new ChangeUserPasswordCommand
            {
                UserId = 1,
                CurrentPassword = "",
                NewPassword = new string('a', 6)
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new ChangeUserPasswordCommand
            {
                UserId = 0,
                CurrentPassword = "",
                NewPassword = new string('a', 6)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(51)]
        public void InvalidNewPasswordLength_DoesNotPassValidation(int len)
        {
            var request = new ChangeUserPasswordCommand
            {
                UserId = 1,
                CurrentPassword = "",
                NewPassword = new string('a', len)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
