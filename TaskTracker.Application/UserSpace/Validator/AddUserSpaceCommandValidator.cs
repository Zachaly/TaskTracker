using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddUserSpaceCommandValidator : AbstractValidator<AddUserSpaceCommand>
    {
        public AddUserSpaceCommandValidator()
        {
            RuleFor(r => r.UserId).GreaterThan(0);
            RuleFor(r => r.StatusGroupId).GreaterThan(0);
            RuleFor(r => r.Title).NotEmpty().Length(1, 100);
        }
    }
}
