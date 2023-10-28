using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.UserTask;

namespace TaskTracker.Application.Command
{
    public class GetUserTaskByIdQuery : IRequest<UserTaskModel>
    {
        public long Id { get; set; }
    }

    public class GetUserTaskByIdHandler : IRequestHandler<GetUserTaskByIdQuery, UserTaskModel>
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public GetUserTaskByIdHandler(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository;
        }

        public Task<UserTaskModel> Handle(GetUserTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return _userTaskRepository.GetByIdAsync(request.Id);
        }
    }
}
