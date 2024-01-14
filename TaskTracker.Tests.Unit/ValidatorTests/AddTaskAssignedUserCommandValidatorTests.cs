using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddTaskAssignedUserCommandValidatorTests
    {
        private readonly AddTaskAssignedUserCommandValidator _validator;

        public AddTaskAssignedUserCommandValidatorTests()
        {
            _validator = new AddTaskAssignedUserCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddTaskAssignedUserCommand
            {
                TaskId = 1,
                UserId = 2,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidTaskId_DoesNotPassValidation()
        {
            var request = new AddTaskAssignedUserCommand
            {
                TaskId = -1,
                UserId = 2,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddTaskAssignedUserCommand
            {
                TaskId = 1,
                UserId = -2,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
