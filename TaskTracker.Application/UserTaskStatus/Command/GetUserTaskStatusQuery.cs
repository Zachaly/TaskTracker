using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.UserTaskStatus;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskStatusQuery : GetUserTaskStatusRequest, IRequest<IEnumerable<UserTaskStatusModel>>
    {
    }

    public class GetUserTaskStatusHandler : IRequestHandler<GetUserTaskStatusQuery, IEnumerable<UserTaskStatusModel>>
    {
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;

        public GetUserTaskStatusHandler(IUserTaskStatusRepository userTaskStatusRepository)
        {
            _userTaskStatusRepository = userTaskStatusRepository;
        }

        public Task<IEnumerable<UserTaskStatusModel>> Handle(GetUserTaskStatusQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
