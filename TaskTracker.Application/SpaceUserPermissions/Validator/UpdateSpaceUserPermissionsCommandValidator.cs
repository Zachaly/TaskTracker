using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateSpaceUserPermissionsCommandValidator : AbstractValidator<UpdateSpaceUserPermissionsCommand>
    {
        public UpdateSpaceUserPermissionsCommandValidator()
        {
            RuleFor(r => r.UserId).GreaterThan(0);
            RuleFor(r => r.SpaceId).GreaterThan(0);
        }
    }
}
