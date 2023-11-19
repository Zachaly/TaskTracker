using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddUserTaskStatusCommandValidatorTests
    {
        private readonly AddUserTaskStatusCommandValidator _validator;

        public AddUserTaskStatusCommandValidatorTests()
        {
            _validator = new AddUserTaskStatusCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddUserTaskStatusCommand
            {
                Index = 1,
                Color = "blue",
                GroupId = 2,
                Name = "Test",
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(21)]
        public void InvalidIndex_DoesNotPassValidation(int index)
        {
            var request = new AddUserTaskStatusCommand
            {
                Index = index,
                Color = "blue",
                GroupId = 2,
                Name = "Test",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        public void InvalidColorLength_DoesNotPassValidation(int len)
        {
            var request = new AddUserTaskStatusCommand
            {
                Index = 1,
                Color = new string('a', len),
                GroupId = 2,
                Name = "Test",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidGroupId_DoesNotPassValidation()
        {
            var request = new AddUserTaskStatusCommand
            {
                Index = 1,
                Color = "blue",
                GroupId = 0,
                Name = "Test",
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidNameLength_DoesNotPassValidation(int len)
        {
            var request = new AddUserTaskStatusCommand
            {
                Index = 1,
                Color = "blue",
                GroupId = 2,
                Name = new string('a', len),
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
