using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Api.Infrastructure.Command;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Controllers
{
    [Route("api/image")]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns profile picture of specified user or default if user doesn't have one
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("user/{id}")]
        public async Task<FileStreamResult> GetProfilePictureAsync(long id)
        {
            var res = await _mediator.Send(new GetProfilePictureQuery { UserId = id });

            return new FileStreamResult(res, "image/png");
        }

        /// <summary>
        /// Updates profile picture of user
        /// </summary>
        /// <response code="204">Picture changed successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost("user")]
        [ProducesResponseType(204)]
        [Authorize]
        public async Task<ActionResult<ResponseModel>> ChangeProfilePictureAsync([FromForm] SaveProfilePictureCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
