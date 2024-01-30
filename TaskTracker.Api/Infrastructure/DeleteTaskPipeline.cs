using MediatR;
using TaskTracker.Api.Infrastructure.Command;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskFileAttachment.Request;

namespace TaskTracker.Api.Infrastructure
{
    public class DeleteTaskPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : DeleteUserTaskByIdCommand
        where TResponse : ResponseModel
    {
        private readonly ITaskFileAttachmentRepository _taskFileAttachmentRepository;
        private readonly IMediator _mediator;

        public DeleteTaskPipeline(ITaskFileAttachmentRepository taskFileAttachmentRepository, IMediator mediator)
        {
            _taskFileAttachmentRepository = taskFileAttachmentRepository;
            _mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var fileAttachments = await _taskFileAttachmentRepository.GetAsync(new GetTaskFileAttachmentRequest
            {
                TaskId = request.Id,
                SkipPagination = true
            });

            foreach(var attachment in fileAttachments.ToList()) 
            {
                await _mediator.Send(new DeleteTaskFileAttachmentByIdCommand(attachment.Id));
            }

            return await next();
        }
    }
}
