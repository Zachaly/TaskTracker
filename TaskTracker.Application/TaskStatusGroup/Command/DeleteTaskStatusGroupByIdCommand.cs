using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public class DeleteTaskStatusGroupByIdCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteTaskStatusGroupByIdHandler : IRequestHandler<DeleteTaskStatusGroupByIdCommand, ResponseModel>
    {
        private readonly ITaskStatusGroupRepository _taskStatusGroupRepository;

        public DeleteTaskStatusGroupByIdHandler(ITaskStatusGroupRepository taskStatusGroupRepository)
        {
            _taskStatusGroupRepository = taskStatusGroupRepository;
        }

        public Task<ResponseModel> Handle(DeleteTaskStatusGroupByIdCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
