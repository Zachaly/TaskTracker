using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).MaximumLength(50);
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
