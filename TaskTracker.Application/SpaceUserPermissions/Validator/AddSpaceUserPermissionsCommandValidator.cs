using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddSpaceUserPermissionsCommandValidator : AbstractValidator<AddSpaceUserPermissionsCommand>
    {
        public AddSpaceUserPermissionsCommandValidator()
        {
            RuleFor(r => r.UserId).GreaterThan(0);
            RuleFor(r => r.SpaceId).GreaterThan(0);
        }
    }
}
