using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.Command;
using TaskTracker.Application.Validation;

namespace TaskTracker.Tests.Unit.ValidatorTests
{
    public class AddSpaceUserCommandValidatorTests
    {
        private readonly AddSpaceUserCommandValidator _validator;

        public AddSpaceUserCommandValidatorTests()
        {
            _validator = new AddSpaceUserCommandValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddSpaceUserCommand
            {
                SpaceId = 1,
                UserId = 2,
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddSpaceUserCommand
            {
                SpaceId = 1,
                UserId = -2,
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidSpaceId_DoesNotPassValidation()
        {
            var request = new AddSpaceUserCommand
            {
                SpaceId = -1,
                UserId = 2,
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
