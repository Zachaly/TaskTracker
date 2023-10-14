using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Controllers
{
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates new user with data given in request
        /// </summary>
        /// <response code="201">User created</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreatedResponseModel>> RegisterAsync(RegisterCommand registerCommand)
        {
            var response = await _mediator.Send(registerCommand);

            return response.ReturnCreatedOrBadRequest("api/user");
        }
    }
}
