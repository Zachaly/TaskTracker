using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
    {
        public UpdateDocumentCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);
            RuleFor(r => r.Title).MaximumLength(200);
        }
    }
}
