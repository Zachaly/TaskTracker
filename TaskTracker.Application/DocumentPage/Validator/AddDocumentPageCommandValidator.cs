using FluentValidation;
using TaskTracker.Application.Command;

namespace TaskTracker.Application.Validator
{
    public class AddDocumentPageCommandValidator : AbstractValidator<AddDocumentPageCommand>
    {
        public AddDocumentPageCommandValidator()
        {
            RuleFor(r => r.DocumentId).GreaterThan(0);
            RuleFor(r => r.Title).MaximumLength(200);
            RuleFor(r => r.Content).MaximumLength(10000);
        }
    }
}
