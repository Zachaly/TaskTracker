using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.User;

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

        /// <summary>
        /// Returns user with specified id
        /// </summary>
        /// <response code="200">User model</response>
        /// <response code="404">No user with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetUserByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        } 

        /// <summary>
        /// Updates user with data given in request
        /// </summary>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
