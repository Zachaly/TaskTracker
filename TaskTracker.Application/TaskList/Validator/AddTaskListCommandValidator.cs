using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddTaskListCommandValidator : AbstractValidator<AddTaskListCommand>
    {
        public AddTaskListCommandValidator()
        {
            RuleFor(r => r.Title).NotEmpty().MaximumLength(200);
            RuleFor(r => r.Description).MaximumLength(1000);
            RuleFor(r => r.CreatorId).GreaterThan(0);
            RuleFor(r => r.Color).MaximumLength(10);
        }
    }
}
