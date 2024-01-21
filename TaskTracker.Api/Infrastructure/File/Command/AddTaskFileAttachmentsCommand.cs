using MediatR;
using TaskTracker.Application;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Infrastructure.Command
{

    public class AddTaskFileAttachmentsCommand : IRequest<ResponseModel>
    {
        public long TaskId { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }

    public class AddTaskFileAttachmentsHandler : IRequestHandler<AddTaskFileAttachmentsCommand, ResponseModel>
    {
        private readonly ITaskFileAttachmentRepository _repository;
        private readonly ITaskFileAttachmentFactory _factory;
        private readonly IFileService _fileService;

        public AddTaskFileAttachmentsHandler(ITaskFileAttachmentRepository repository, ITaskFileAttachmentFactory factory, 
            IFileService fileService)
        {
            _repository = repository;
            _factory = factory;
            _fileService = fileService;
        }

        public async Task<ResponseModel> Handle(AddTaskFileAttachmentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var names = await _fileService.SaveTaskFileAttachmentsAsync(request.Files.ToArray());

                var attachments = names.Select(name => _factory.Create(request.TaskId, name)).ToArray();

                await _repository.AddAsync(attachments);
            }
            catch(Exception ex)
            {
                return new ResponseModel(ex.Message);
            }
            
            return new ResponseModel();
        }
    }
}
