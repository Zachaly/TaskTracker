using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.SpaceUserPermissions;

namespace TaskTracker.Api.Controllers
{
    [Authorize]
    [Route("api/space-user-permissions")]
    public class SpaceUserPermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpaceUserPermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of space user permissions filtered by request
        /// </summary>
        /// <response code="200">List of user permissions</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<SpaceUserPermissionsModel>>> GetAsync([FromQuery] GetSpaceUserPermissionsQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Adds new space user permissions with data given in request
        /// </summary>
        /// <response code="204">Permissions created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [SpacePermissionRequired(SpacePermissionTypes.CanChangePermissions)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddSpaceUserPermissionsCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Updates space user permissions with data given in request
        /// </summary>
        /// <response code="204">Permissions updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [SpacePermissionRequired(SpacePermissionTypes.CanChangePermissions)]
        public async Task<ActionResult<ResponseModel>> PutAsync(UpdateSpaceUserPermissionsCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
