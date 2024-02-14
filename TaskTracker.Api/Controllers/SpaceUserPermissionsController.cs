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

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<SpaceUserPermissionsModel>>> GetAsync([FromQuery] GetSpaceUserPermissionsQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddSpaceUserPermissionsCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PutAsync(UpdateSpaceUserPermissionsCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
