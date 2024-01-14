using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddTaskAssignedUserCommandValidator : AbstractValidator<AddTaskAssignedUserCommand>
    {
        public AddTaskAssignedUserCommandValidator()
        {
            RuleFor(r => r.UserId).GreaterThan(0);
            RuleFor(r => r.TaskId).GreaterThan(0);
        }
    }
}
