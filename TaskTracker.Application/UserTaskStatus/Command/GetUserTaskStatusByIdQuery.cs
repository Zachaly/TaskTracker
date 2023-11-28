using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.UserTaskStatus;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskStatusByIdQuery : IRequest<UserTaskStatusModel?>
    {
        public long Id { get; set; }
    }

    public class GetUserTaskStatusByIdHandler : IRequestHandler<GetUserTaskStatusByIdQuery, UserTaskStatusModel?> 
    {
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;

        public GetUserTaskStatusByIdHandler(IUserTaskStatusRepository userTaskStatusRepository)
        {
            _userTaskStatusRepository = userTaskStatusRepository;
        }

        public Task<UserTaskStatusModel?> Handle(GetUserTaskStatusByIdQuery request, CancellationToken cancellationToken)
        {
            return _userTaskStatusRepository.GetByIdAsync(request.Id);
        }
    }
}
