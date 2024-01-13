using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validation
{
    public class AddSpaceUserCommandValidator : AbstractValidator<AddSpaceUserCommand>
    {
        public AddSpaceUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.SpaceId).GreaterThan(0);
        }
    }
}
