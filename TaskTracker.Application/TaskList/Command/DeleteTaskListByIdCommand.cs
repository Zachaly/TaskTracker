using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Application.Command
{
    public class DeleteTaskListByIdCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteTaskListByIdHandler : IRequestHandler<DeleteTaskListByIdCommand, ResponseModel>
    {
        public DeleteTaskListByIdHandler(ITaskListRepository taskListRepository)
        {
            
        }

        public Task<ResponseModel> Handle(DeleteTaskListByIdCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
