using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Api.Controllers
{
    [Route("api/user-task")]
    [Authorize]
    public class UserTaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of user tasks filtered by data in query
        /// </summary>
        /// <response code="200">List of user tasks</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTaskModel>>> GetAsync([FromQuery] GetUserTaskQuery query)
        {
            var res = await _mediator.Send(query);

            return new OkObjectResult(res);
        }

        /// <summary>
        /// Returns user task with specified id
        /// </summary>
        /// <response code="200">User task</response>
        /// <response code="404">No user task with specified id</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTaskModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetUserTaskByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Creates new user task with data given in request
        /// </summary>
        /// <response code="201">Task created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [SpacePermissionRequired(SpacePermissionTypes.CanModifyTasks)]
        public async Task<ActionResult<CreatedResponseModel>> PostAsync(AddUserTaskCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("user-task");
        }

        /// <summary>
        /// Removed user task with specified id
        /// </summary>
        /// <response code="204">User task deleted successfully</response>
        /// <response code="400">No task with specified id exists</response>
        [HttpDelete("{id}")]
        [SpacePermissionRequired(SpacePermissionTypes.CanRemoveTasks)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteUserTaskByIdCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Updates user task with data specified in request
        /// </summary>
        /// <response code="204">Task updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [SpacePermissionRequired(SpacePermissionTypes.CanModifyTasks)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateUserTaskCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
