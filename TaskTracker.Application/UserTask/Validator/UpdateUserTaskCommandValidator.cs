using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateUserTaskCommandValidator : AbstractValidator<UpdateUserTaskCommand>
    {
        public UpdateUserTaskCommandValidator()
        {
            RuleFor(t => t.Id).GreaterThan(0);
            RuleFor(t => t.DueTimestamp).GreaterThanOrEqualTo(0);
            RuleFor(t => t.Description).MaximumLength(1000);
            RuleFor(t => t.Title).MaximumLength(200);
        }
    }
}
