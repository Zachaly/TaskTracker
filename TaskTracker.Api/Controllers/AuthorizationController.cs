using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Authorization.Command;
using TaskTracker.Model.User;

namespace TaskTracker.Api.Controllers
{
    [Route("api/auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns tokens and user data of specified user
        /// </summary>
        /// <response code="200">Token and user data</response>
        /// <response code="400">Invalid credentials</response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginCommand command)
        {
            var response = await _mediator.Send(command);

            return response.ReturnOkOrBadRequest();
        }
    }
}
