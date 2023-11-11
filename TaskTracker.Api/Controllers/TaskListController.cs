using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskList;

namespace TaskTracker.Api.Controllers
{
    [Authorize]
    [Route("api/task-list")]
    public class TaskListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of task lists filtered by request
        /// </summary>
        /// <response code="200">List of task list info</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<TaskListModel>>> GetAsync([FromQuery] GetTaskListQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns task list with specified id
        /// </summary>
        /// <response code="200">Task list info</response>
        /// <response code="404">No task list with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskListModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetTaskListByIdQuery { Id = id });

            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Adds new task list with data given in request
        /// </summary>
        /// <response code="201">Task list created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreatedResponseModel>> PostAsync(AddTaskListCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("task-list");
        }

        /// <summary>
        /// Updates task list with data given in request
        /// </summary>
        /// <response code="204">Task list updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PutAsync(UpdateTaskListCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes task list with specified id, also deletes all tasks related to this list
        /// </summary>
        /// <response code="204">Task list deleted successfully</response>
        /// <response code="400">Invalid list id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteTaskListByIdCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
