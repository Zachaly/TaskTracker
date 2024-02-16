using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.SpaceUser;

namespace TaskTracker.Api.Controllers
{
    [Authorize]
    [Route("api/space-user")]
    public class SpaceUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpaceUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of space users filtered by request
        /// </summary>
        /// <response code="200">List of space users</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<SpaceUserModel>>> GetAsync([FromQuery] GetSpaceUserQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Creates new space user using data given in request
        /// </summary>
        /// <response code="204">User created successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [SpacePermissionRequired(SpacePermissionTypes.CanAddUsers)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddSpaceUserCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes space user with specified space and user id
        /// </summary>
        /// <response code="204">User deleted successfully</response>
        /// <response code="400">No user with specified ids found</response>
        [HttpDelete("{spaceId}/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [SpacePermissionRequired(SpacePermissionTypes.CanRemoveUsers)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long spaceId, long userId)
        {
            var res = await _mediator.Send(new DeleteSpaceUserCommand(spaceId, userId));

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
