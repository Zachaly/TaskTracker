using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskQuery : IRequest<IEnumerable<UserTaskModel>>
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
            throw new NotImplementedException();
        }
    }
}
