using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class UpdateUserTaskStatusCommandValidatorTests
    {
        private readonly UpdateUserTaskStatusCommandValidator _validator;

        public UpdateUserTaskStatusCommandValidatorTests()
        {
            _validator = new UpdateUserTaskStatusCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateUserTaskStatusCommand
            {
                Id = 1,
                Color = "blue",
                Index = 2,
                Name = "name",
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ValidRequest_OnlyRequiredFields_PassesValidation()
        {
            var request = new UpdateUserTaskStatusCommand
            {
                Id = 1,
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateUserTaskStatusCommand
            {
                Id = 0,
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(21)]
        public void InvalidIndex_DoesNotPassValidation(int index)
        {
            var request = new UpdateUserTaskStatusCommand
            {
                Id = 1,
                Index = index,
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        public void InvalidColorLength_DoesNotPassValidation(int len)
        {
            var request = new UpdateUserTaskStatusCommand
            {
                Id = 1,
                Color = new string('a', len),
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidNameLength_DoesNotPassValidation(int len)
        {
            var request = new UpdateUserTaskStatusCommand
            {
                Id = 1,
                Name = new string('a', len),
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
