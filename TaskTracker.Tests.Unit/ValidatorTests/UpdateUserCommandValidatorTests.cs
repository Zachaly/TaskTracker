using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class UpdateUserCommandValidatorTests
    {
        private readonly UpdateUserCommandValidator _validator;

        public UpdateUserCommandValidatorTests()
        {
            _validator = new UpdateUserCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateUserCommand
            {
                Id = 1,
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateUserCommand
            {
                Id = 0
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidFirstNameLength_DoesNotPassValidation(int len)
        {
            var request = new UpdateUserCommand
            {
                Id = 1,
                FirstName = new string('a', len)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidLastNameLength_DoesNotPassValidation(int len)
        {
            var request = new UpdateUserCommand
            {
                Id = 1,
                LastName = new string('a', len)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
