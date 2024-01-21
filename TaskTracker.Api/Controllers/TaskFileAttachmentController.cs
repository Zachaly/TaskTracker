using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Api.Infrastructure.Command;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskFileAttachment;

namespace TaskTracker.Api.Controllers
{
    [Route("/api/task-file-attachment")]
    [Authorize]
    public class TaskFileAttachmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskFileAttachmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of task file attachment filtered by request
        /// </summary>
        /// <response code="200">List of attachments</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<TaskFileAttachmentModel>>> GetAsync([FromQuery] GetTaskFileAttachmentQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns task file attachment with specified id
        /// </summary>
        /// <response code="200">Task file attachment</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskFileAttachmentModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetTaskFileAttachmentByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Creates new task file attachments with data given in request
        /// </summary>
        /// <response code="204">Attachments added successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync([FromForm] AddTaskFileAttachmentsCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes task file attachment with specified id
        /// </summary>
        /// <response code="204">Attachment deleted successfully</response>
        /// <response code="400">File not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteTaskFileAttachmentByIdCommand(id));

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns stream of task file attachment with specified id
        /// </summary>
        /// <response code="200">File stream</response>
        /// <response code="404">File not found</response>
        [HttpGet("file/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetFileByIdAsync(long id)
        {
            var res = await _mediator.Send(new StreamTaskFileAttachmentQuery(id));

            if(!res.IsSuccess)
            {
                return new BadRequestObjectResult(res);
            }

            return File(res.Stream!, "application/octet-stream", $"{res.FileName}");
        }
    }
}
