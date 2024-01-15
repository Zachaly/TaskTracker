using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskAssignedUser;

namespace TaskTracker.Api.Controllers
{
    [Authorize]
    [Route("/api/task-assigned-user")]
    public class TaskAssignedUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskAssignedUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of task assigned users filtered by query
        /// </summary>
        /// <response code="200">List of users</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<TaskAssignedUserModel>>> GetAsync([FromQuery] GetTaskAssignedUserQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Creates new task assigned user with data given in request
        /// </summary>
        /// <response code="204">User created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddTaskAssignedUserCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes task assigned user with specified task id and user id
        /// </summary>
        /// <response code="204">User deleted successfully</response>
        /// <response code="400">No user with specified ids found</response>
        [HttpDelete("{taskId}/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(long taskId, long userId)
        {
            var res = await _mediator.Send(new DeleteTaskAssignedUserCommand(taskId, userId));

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
