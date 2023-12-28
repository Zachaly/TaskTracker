using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Document.Request;
using TaskTracker.Model.DocumentPage.Request;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public class AddDocumentCommand : AddDocumentRequest, IRequest<CreatedResponseModel>
    {
    }

    public class AddDocumentHandler : IRequestHandler<AddDocumentCommand, CreatedResponseModel>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentFactory _documentFactory;
        private readonly IValidator<AddDocumentCommand> _validator;
        private readonly IDocumentPageRepository _documentPageRepository;
        private readonly IDocumentPageFactory _documentPageFactory;

        public AddDocumentHandler(IDocumentRepository documentRepository, IDocumentFactory documentFactory,
            IValidator<AddDocumentCommand> validator, IDocumentPageRepository documentPageRepository,
            IDocumentPageFactory documentPageFactory)
        {
            _documentRepository = documentRepository;
            _documentFactory = documentFactory;
            _validator = validator;
            _documentPageRepository = documentPageRepository;
            _documentPageFactory = documentPageFactory;
        }

        public async Task<CreatedResponseModel> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var document = _documentFactory.Create(request);

            var id = await _documentRepository.AddAsync(document);

            var defaultPage = _documentPageFactory.Create(new AddDocumentPageRequest
            {
                Content = "",
                DocumentId = id,
                Title = null
            });

            await _documentPageRepository.AddAsync(defaultPage);

            return new CreatedResponseModel(id);
        }
    }
}
