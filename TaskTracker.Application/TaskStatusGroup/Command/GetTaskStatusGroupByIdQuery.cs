using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.TaskStatusGroup;

namespace TaskTracker.Application.Command
{
    public class GetTaskStatusGroupByIdQuery : IRequest<TaskStatusGroupModel>
    {
        public long Id { get; set; }
    }

    public class GetTaskStatusGroupByIdHandler : IRequestHandler<GetTaskStatusGroupByIdQuery, TaskStatusGroupModel>
    {
        private readonly ITaskStatusGroupRepository _taskStatusGroupRepository;

        public GetTaskStatusGroupByIdHandler(ITaskStatusGroupRepository taskStatusGroupRepository)
        {
            _taskStatusGroupRepository = taskStatusGroupRepository;
        }

        public Task<TaskStatusGroupModel> Handle(GetTaskStatusGroupByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
