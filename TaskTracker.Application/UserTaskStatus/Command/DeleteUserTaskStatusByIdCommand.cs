using MediatR;
using TaskTracker.Database.Exception;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class DeleteUserTaskStatusByIdCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteUserTaskStatusByIdHandler : IRequestHandler<DeleteUserTaskStatusByIdCommand, ResponseModel>
    {
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;

        public DeleteUserTaskStatusByIdHandler(IUserTaskStatusRepository userTaskStatusRepository)
        {
            _userTaskStatusRepository = userTaskStatusRepository;
        }

        public async Task<ResponseModel> Handle(DeleteUserTaskStatusByIdCommand request, CancellationToken cancellationToken)
        {
            var isDefault = await _userTaskStatusRepository.GetByIdAsync(request.Id, x => x.IsDefault);

            if(isDefault)
            {
                return new ResponseModel("This status cannot be deleted");
            }

            try 
            {
                var status = await _userTaskStatusRepository.GetByIdAsync(request.Id, x => x);

                await _userTaskStatusRepository.DeleteByIdAsync(request.Id);

                await UpdateGroupStatuses(status);
            }
            catch(EntityNotFoundException ex)
            {
                return new ResponseModel(ex.Message);
            }

            return new ResponseModel();
        }

        private async Task UpdateGroupStatuses(UserTaskStatus deletedStatus)
        {
            var statuses = await _userTaskStatusRepository.GetAsync(new GetUserTaskStatusRequest
            {
                GroupId = deletedStatus.GroupId,
                SkipPagination = true,
                IsDefault = false
            }, x => x);

            foreach(var status in statuses.ToList().Where(x => x.Index >= deletedStatus.Index && x.Id != deletedStatus.Id))
            {
                status.Index--;
                await _userTaskStatusRepository.UpdateAsync(status);
            }
        }
    }
}
