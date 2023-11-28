using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddTaskStatusGroupCommandValidator : AbstractValidator<AddTaskStatusGroupCommand>
    {
        public AddTaskStatusGroupCommandValidator()
        {
            RuleFor(r => r.UserId).GreaterThan(0);
            RuleFor(r => r.Name).NotEmpty().MaximumLength(100);
        }
    }
}
