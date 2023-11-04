using MediatR;
using TaskTracker.Database.Exception;
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
        private ITaskListRepository _taskListRepository;

        public DeleteTaskListByIdHandler(ITaskListRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        public async Task<ResponseModel> Handle(DeleteTaskListByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _taskListRepository.DeleteByIdAsync(request.Id);

                return new ResponseModel();
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }
        }
    }
}
