using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(r => r.FirstName).MinimumLength(1).MaximumLength(100);
            RuleFor(r => r.LastName).MinimumLength(1).MaximumLength(100);
            RuleFor(r => r.Id).GreaterThan(0);
        }
    }
}
