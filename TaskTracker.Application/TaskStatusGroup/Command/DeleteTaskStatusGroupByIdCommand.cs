using MediatR;
using TaskTracker.Database.Exception;
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

        public async Task<ResponseModel> Handle(DeleteTaskStatusGroupByIdCommand request, CancellationToken cancellationToken)
        {
            var isDefault = await _taskStatusGroupRepository.GetByIdAsync(request.Id, x => x.IsDefault);

            if (isDefault)
            {
                return new ResponseModel("This status group cannot be deleted");
            }

            try
            {
                await _taskStatusGroupRepository.DeleteByIdAsync(request.Id);
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }

            return new ResponseModel();
        }
    }
}
