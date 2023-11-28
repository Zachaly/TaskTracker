using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateTaskStatusGroupCommandValidator : AbstractValidator<UpdateTaskStatusGroupCommand>
    {
        public UpdateTaskStatusGroupCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);
            RuleFor(r => r.Name).Length(1, 100);
        }
    }
}
