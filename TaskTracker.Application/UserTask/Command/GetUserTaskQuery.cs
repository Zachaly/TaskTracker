using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.UserTask;
using TaskTracker.Model.UserTask.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskQuery : GetUserTaskRequest, IRequest<IEnumerable<UserTaskModel>>
    {
    }

    public class GetUserTaskHandler : IRequestHandler<GetUserTaskQuery, IEnumerable<UserTaskModel>>
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public GetUserTaskHandler(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository;
        }

        public Task<IEnumerable<UserTaskModel>> Handle(GetUserTaskQuery request, CancellationToken cancellationToken)
        {
            return _userTaskRepository.GetAsync(request);
        }
    }
}
