using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class UpdateTaskStatusGroupCommandValidatorTests
    {
        private readonly UpdateTaskStatusGroupCommandValidator _validator;

        public UpdateTaskStatusGroupCommandValidatorTests()
        {
            _validator = new UpdateTaskStatusGroupCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateTaskStatusGroupCommand
            {
                Id = 1,
                Name = "name"
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ValidRequest_OnlyRequiredFields_PassesValidation()
        {
            var request = new UpdateTaskStatusGroupCommand { Id = 1 };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateTaskStatusGroupCommand 
            { 
                Id = 0
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidNameLength_DoesNotPassValidation(int len)
        {
            var request = new UpdateTaskStatusGroupCommand
            {
                Id = 1,
                Name = new string('a', len)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
