using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateTaskListCommandValidator : AbstractValidator<UpdateTaskListCommand>
    {
        public UpdateTaskListCommandValidator()
        {
            RuleFor(r => r.Title).NotEmpty().MaximumLength(200);
            RuleFor(r => r.Description).MaximumLength(1000);
            RuleFor(r => r.Color).MaximumLength(10);
            RuleFor(r => r.Id).GreaterThan(0);
        }
    }
}
