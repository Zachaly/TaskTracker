using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateUserSpaceCommandValidator : AbstractValidator<UpdateUserSpaceCommand>
    {
        public UpdateUserSpaceCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);
            RuleFor(r => r.Title).MinimumLength(1).MaximumLength(100);
        }
    }
}
