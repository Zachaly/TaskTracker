using FluentValidation;
using TaskTracker.Application.DocumentPage.Command;

namespace TaskTracker.Application.Validator
{
    public class UpdateDocumentPageCommandValidator : AbstractValidator<UpdateDocumentPageCommand>
    {
        public UpdateDocumentPageCommandValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);
            RuleFor(r => r.Title).MaximumLength(200);
            RuleFor(r => r.Content).MaximumLength(10000);
        }
    }
}
