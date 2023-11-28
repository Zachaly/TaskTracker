using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddTaskStatusGroupCommandValidatorTests
    {
        private readonly AddTaskStatusGroupCommandValidator _validator;

        public AddTaskStatusGroupCommandValidatorTests()
        {
            _validator = new AddTaskStatusGroupCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddTaskStatusGroupCommand
            {
                Name = "name",
                UserId = 1
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidNameLength_DoesNotPassValidation(int len)
        {
            var request = new AddTaskStatusGroupCommand
            {
                UserId = 1,
                Name = new string('a', len)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddTaskStatusGroupCommand
            {
                Name = "name",
                UserId = 0
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
