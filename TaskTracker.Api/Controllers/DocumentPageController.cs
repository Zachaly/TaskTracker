using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Application.DocumentPage.Command;
using TaskTracker.Model.DocumentPage;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Controllers
{
    [Authorize]
    [Route("/api/document-page")]
    public class DocumentPageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentPageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of document pages filtered by request
        /// </summary>
        /// <response code="200">List of pages</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<DocumentPageModel>>> GetAsync([FromQuery] GetDocumentPageQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns document page with specified id
        /// </summary>
        /// <response code="200">Page model</response>
        /// <response code="404">No page with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DocumentPageModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetDocumentPageByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new document page with data given in request
        /// </summary>
        /// <response code="201">Page created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreatedResponseModel>> PostAsync(AddDocumentPageCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("document-page");
        }

        /// <summary>
        /// Updates document page with data given in request
        /// </summary>
        /// <response code="204">Page updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateDocumentPageCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes document page with specified id
        /// </summary>
        /// <response code="204">Page deleted successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteDocumentPageByIdCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
