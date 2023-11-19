using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddUserTaskStatusCommandValidator : AbstractValidator<AddUserTaskStatusCommand>
    {
        public AddUserTaskStatusCommandValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(100);
            RuleFor(r => r.Color).NotEmpty().MaximumLength(10);
            RuleFor(r => r.Index).GreaterThan(0).LessThan(21);
            RuleFor(r => r.GroupId).GreaterThan(0);
        }
    }
}
