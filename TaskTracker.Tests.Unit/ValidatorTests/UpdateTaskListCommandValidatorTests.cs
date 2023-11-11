using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.Command;
using TaskTracker.Application.Validator;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class UpdateTaskListCommandValidatorTests
    {
        private readonly UpdateTaskListCommandValidator _validator;

        public UpdateTaskListCommandValidatorTests()
        {
            _validator = new UpdateTaskListCommandValidator();
        }

        [Fact]
        public async Task ValidRequest_PassesValidation()
        {
            var request = new UpdateTaskListCommand
            {
                Id = 1,
                Title = "Test",
            };

            var res = await _validator.ValidateAsync(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public async Task InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateTaskListCommand
            {
                Id = 0,
                Title = "title"
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(201)]
        public async Task InvalidTitleLength_DoesNotPassValidation(int length)
        {
            var request = new UpdateTaskListCommand
            {
                Id = 1,
                Title = new string('a', length)
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public async Task InvalidColorLength_DoesNotPassValidation()
        {
            var request = new UpdateTaskListCommand
            {
                Id = 1,
                Title = "title",
                Color = new string('a', 11)
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public async Task InvalidDescriptionLength_DoesNotPassValidation()
        {
            var request = new UpdateTaskListCommand
            {
                Id = 1,
                Title = "title",
                Description = new string('a', 1001)
            };

            var res = await _validator.ValidateAsync(request);

            Assert.False(res.IsValid);
        }
    }
}
