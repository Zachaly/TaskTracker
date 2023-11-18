using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.TaskStatusGroup;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application.Command
{
    public class GetTaskStatusGroupQuery : GetTaskStatusGroupRequest, IRequest<IEnumerable<TaskStatusGroupModel>>
    {
    }

    public class GetTaskStatusGroupHandler : IRequestHandler<GetTaskStatusGroupQuery, IEnumerable<TaskStatusGroupModel>>
    {
        private readonly ITaskStatusGroupRepository _taskStatusGroupRepository;

        public GetTaskStatusGroupHandler(ITaskStatusGroupRepository taskStatusGroupRepository)
        {
            _taskStatusGroupRepository = taskStatusGroupRepository;
        }

        public Task<IEnumerable<TaskStatusGroupModel>> Handle(GetTaskStatusGroupQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
