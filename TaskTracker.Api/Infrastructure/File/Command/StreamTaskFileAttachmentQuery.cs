using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Infrastructure.Command
{
    public class TaskFileAttachmentStreamResponse : ResponseModel
    {
        public Stream? Stream { get; set; }

        public TaskFileAttachmentStreamResponse(Stream stream) : base()
        {
            Stream = stream;
        }

        public TaskFileAttachmentStreamResponse(string error) : base(error)
        {
            Stream = null;
        }
    }

    public record StreamTaskFileAttachmentQuery(long Id) : IRequest<TaskFileAttachmentStreamResponse>
    {
    }

    public class StreamTaskFileAttachmentHandler : IRequestHandler<StreamTaskFileAttachmentQuery, TaskFileAttachmentStreamResponse>
    {
        private readonly ITaskFileAttachmentRepository _repository;
        private readonly IFileService _fileService;

        public StreamTaskFileAttachmentHandler(ITaskFileAttachmentRepository repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<TaskFileAttachmentStreamResponse> Handle(StreamTaskFileAttachmentQuery request, CancellationToken cancellationToken)
        {
            var file = await _repository.GetByIdAsync(request.Id);

            if(file is null)
            {
                return new TaskFileAttachmentStreamResponse("File not found");
            }

            return new TaskFileAttachmentStreamResponse(
                await _fileService.GetTaskFileAttachmentAsync(file.FileName));
        }
    }
}
