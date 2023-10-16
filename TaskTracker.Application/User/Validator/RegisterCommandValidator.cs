using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(50);
            RuleFor(r => r.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(r => r.LastName).NotEmpty().MaximumLength(100);
            RuleFor(r => r.Password).NotEmpty().MaximumLength(50);
        }
    }
}
