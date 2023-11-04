using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.TaskList;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskListQuery : GetTaskListRequest, IRequest<IEnumerable<TaskListModel>>
    {
    }

    public class GetTaskListHandler : IRequestHandler<GetTaskListQuery, IEnumerable<TaskListModel>>
    {
        private readonly ITaskListRepository _taskListRepository;

        public GetTaskListHandler(ITaskListRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        public Task<IEnumerable<TaskListModel>> Handle(GetTaskListQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
