using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Document;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Controllers
{
    [Authorize]
    [Route("/api/document")]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns documents filtered by id
        /// </summary>
        /// <response code="200">List of documents</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<DocumentModel>>> GetAsync([FromQuery] GetDocumentQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns document with specified id
        /// </summary>
        /// <response code="200">Document model</response>
        /// <response code="404">No document with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DocumentModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetDocumentByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new document with empty page using data given in request
        /// </summary>
        /// <response code="201">Document created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreatedResponseModel>> PostAsync(AddDocumentCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("document");
        }

        /// <summary>
        /// Updates document with data given in request
        /// </summary>
        /// <response code="204">Document updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateDocumentCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes document with specified id
        /// </summary>
        /// <response code="204">Document deleted successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteDocumentByIdCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
