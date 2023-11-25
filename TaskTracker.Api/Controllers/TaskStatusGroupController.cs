using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Application.Command;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskStatusGroup;

namespace TaskTracker.Api.Controllers
{
    [Route("/api/task-status-group")]
    [Authorize]
    public class TaskStatusGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskStatusGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns list of task status groups filtered by query
        /// </summary>
        /// <response code="200">List of status groups</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<TaskStatusGroupModel>>> GetAsync([FromQuery] GetTaskStatusGroupQuery query)
        {
            var res = await _mediator.Send(query);

            return Ok(res);
        }

        /// <summary>
        /// Returns task status group with specified id
        /// </summary>
        /// <response code="200">Task status group model</response>
        /// <response code="404">No status group with specified id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TaskStatusGroupModel>> GetByIdAsync(long id)
        {
            var res = await _mediator.Send(new GetTaskStatusGroupByIdQuery { Id = id });
            
            return ResponseModelExtensions.ReturnOkOrNotFound(res);
        }

        /// <summary>
        /// Creates new task status group with data given in request
        /// </summary>
        /// <response code="201">Task status group created successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddTaskStatusGroupCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnCreatedOrBadRequest("task-status-group");
        }

        /// <summary>
        /// Updates task status group with data given in request
        /// </summary>
        /// <response code="204">Task status group updated successfully</response>
        /// <response code="400">Invalid request</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync(UpdateTaskStatusGroupCommand command)
        {
            var res = await _mediator.Send(command);

            return res.ReturnNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes task status group with specified id
        /// </summary>
        /// <response code="204">Task status group deleted successfully</response>
        /// <response code="400">Invalid id or user is not allowed to delete this status group</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteByIdAsync(long id)
        {
            var res = await _mediator.Send(new DeleteTaskStatusGroupByIdCommand { Id = id });

            return res.ReturnNoContentOrBadRequest();
        }
    }
}
