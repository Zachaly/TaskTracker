using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserSpace;

namespace TaskTracker.Api.Controllers
{
    [Authorize]
    [Route("/api/user-space")]
    public class UserSpaceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserSpaceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of user spaces filtered by query
        /// </summary>
        /// <response code="200">List of spaces</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserSpaceModel>>> GetAsync([FromQuery] GetUserSpaceQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns user space with specified id
        /// </summary>
        /// <response code="200">User space</response>
        /// <response code="404">No user space with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserSpaceModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetUserSpaceByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Creates new user space with data given in request
        /// </summary>
        /// <response code="201">Space created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreatedResponseModel>> PostAsync(AddUserSpaceCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("user-space");
        }

        /// <summary>
        /// Updates user space with data given in request
        /// </summary>
        /// <response code="204">Space updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateUserSpaceCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes user space with specified id
        /// </summary>
        /// <response code="204">User space deleted successfully</response>
        /// <response code="400">Invalid id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteUserSpaceByIdCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
