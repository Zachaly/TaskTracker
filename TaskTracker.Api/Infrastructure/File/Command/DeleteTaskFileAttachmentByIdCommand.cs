using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Infrastructure.Command
{
    public record DeleteTaskFileAttachmentByIdCommand(long Id) : IRequest<ResponseModel>
    {
        
    }

    public class DeleteTaskFileAttachmentByIdHandler : IRequestHandler<DeleteTaskFileAttachmentByIdCommand, ResponseModel>
    {
        private readonly ITaskFileAttachmentRepository _repository;
        private readonly IFileService _fileService;

        public DeleteTaskFileAttachmentByIdHandler(ITaskFileAttachmentRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<ResponseModel> Handle(DeleteTaskFileAttachmentByIdCommand request, CancellationToken cancellationToken)
        {
            var file = await _repository.GetByIdAsync(request.Id);

            if (file is null)
            {
                return new ResponseModel("File not found!");
            }

            await _repository.DeleteByIdAsync(request.Id);

            await _fileService.DeleteTaskFileAttachmentAsync(file.FileName);

            return new ResponseModel();
        }
    }
}
