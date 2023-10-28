using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class RegisterCommandValidatorTests
    {
        private readonly RegisterCommandValidator _validator;

        public RegisterCommandValidatorTests()
        {
            _validator = new RegisterCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                FirstName = "Test",
                LastName = "Test",
                Password = "password",
            };

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidEmail_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Email = "email",
                FirstName = "Test",
                LastName = "Test",
                Password = "password",
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidFirstNameLength_DoesNotPassValidation(int len)
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                FirstName = new string('a', len),
                LastName = "Test",
                Password = "password",
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidLastNameLength_DoesNotPassValidation(int len)
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                FirstName = "test",
                LastName = new string('a', len),
                Password = "password",
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(51)]
        public void InvalidPasswordLength_DoesNotPassValidation(int len)
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                FirstName = "Test",
                LastName = "Test",
                Password = new string('a', len),
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }
    }
}
