using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Authorization.Command;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
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

        /// <summary>
        /// Returns new access token and refresh token based on request
        /// </summary>
        /// <response code="200">New tokens and user data</response>
        /// <response code="400">Invalid access or refresh token</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LoginResponse>> RefreshTokenAsync(RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);

            return response.ReturnOkOrBadRequest();
        }

        /// <summary>
        /// Revokes refresh token given in request
        /// </summary>
        [HttpPut("revoke-token")]
        [ProducesResponseType(204)]
        public async Task<ActionResult<ResponseModel>> RevokeTokenAsync(InvalidateRefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            
            return response.ReturnNoContentOrBadRequest();
        }
    }
}
