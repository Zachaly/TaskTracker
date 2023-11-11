using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.TaskList;

namespace TaskTracker.Application.Command
{
    public class GetTaskListByIdQuery : IRequest<TaskListModel?>
    {
        public long Id { get; set; }
    }

    public class GetTaskListByIdHandler : IRequestHandler<GetTaskListByIdQuery, TaskListModel?>
    {
        private readonly ITaskListRepository _taskListRepository;

        public GetTaskListByIdHandler(ITaskListRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        public Task<TaskListModel?> Handle(GetTaskListByIdQuery request, CancellationToken cancellationToken)
        {
            return _taskListRepository.GetByIdAsync(request.Id);
        }
    }
}
