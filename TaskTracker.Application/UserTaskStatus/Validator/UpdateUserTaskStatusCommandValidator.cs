using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateUserTaskStatusCommandValidator : AbstractValidator<UpdateUserTaskCommand>
    {
    }
}
