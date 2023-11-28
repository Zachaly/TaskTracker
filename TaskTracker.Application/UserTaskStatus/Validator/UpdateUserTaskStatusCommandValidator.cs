using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateUserTaskStatusCommandValidator : AbstractValidator<UpdateUserTaskStatusCommand>
    {
        public UpdateUserTaskStatusCommandValidator()
        {
            RuleFor(r => r.Name).Length(1, 100);
            RuleFor(r => r.Index).GreaterThan(0).LessThan(21);
            RuleFor(r => r.Id).GreaterThan(0);
            RuleFor(r => r.Color).Length(1, 10);
        }
    }
}
