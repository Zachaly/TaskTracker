using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTaskStatus;

namespace TaskTracker.Api.Controllers
{

    [Route("/api/user-task-status")]
    [Authorize]
    public class UserTaskStatusController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserTaskStatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of user task statuses filtered with data from query
        /// </summary>
        /// <response code="200">List of statuses</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserTaskStatusModel>>> GetAsync([FromQuery] GetUserTaskStatusQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns user task status with specified id
        /// </summary>
        /// <response code="200">Task status model</response>
        /// <response code="404">No status with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserTaskStatusModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetUserTaskStatusByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Creates new task status with data given in request
        /// </summary>
        /// <response code="201">Status created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddUserTaskStatusCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("user-task-status");
        }

        /// <summary>
        /// Updates task status with data given in request
        /// </summary>
        /// <response code="204">Status updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PutAsync(UpdateUserTaskStatusCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes user task status with specified id
        /// </summary>
        /// <response code="204">Task status deleted successfully</response>
        /// <response code="400">Invalid id or user is not allowed to delete specified status</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteUserTaskStatusByIdCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
