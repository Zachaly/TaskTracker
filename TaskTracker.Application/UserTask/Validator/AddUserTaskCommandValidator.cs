using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddUserTaskCommandValidator : AbstractValidator<AddUserTaskCommand>
    {
        public AddUserTaskCommandValidator()
        {
            RuleFor(t => t.DueTimestamp).GreaterThanOrEqualTo(0);
            RuleFor(t => t.CreatorId).GreaterThan(0);
            RuleFor(t => t.Description).MaximumLength(1000);
            RuleFor(t => t.Title).NotEmpty().MaximumLength(200);
        }
    }
}
