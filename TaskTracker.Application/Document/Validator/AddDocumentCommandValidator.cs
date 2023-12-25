using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddDocumentCommandValidator : AbstractValidator<AddDocumentCommand>
    {
        public AddDocumentCommandValidator()
        {
            RuleFor(r => r.Title).NotEmpty().MaximumLength(200);
            RuleFor(r => r.SpaceId).GreaterThan(0);
            RuleFor(r => r.CreatorId).GreaterThan(0);
        }
    }
}
