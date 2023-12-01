using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskStatusGroup;

namespace TaskTracker.Application.Command
{
    public class DeleteTaskStatusGroupByIdCommand : DeleteEntityByIdCommand
    {
        
    }

    public class DeleteTaskStatusGroupByIdHandler : DeleteEntityByIdHandler<TaskStatusGroup, TaskStatusGroupModel, DeleteTaskStatusGroupByIdCommand>
    {
        public DeleteTaskStatusGroupByIdHandler(ITaskStatusGroupRepository taskStatusGroupRepository)
            : base(taskStatusGroupRepository)
        {
        }

        public override async Task<ResponseModel> Handle(DeleteTaskStatusGroupByIdCommand request, CancellationToken cancellationToken)
        {
            var isDefault = await _repository.GetByIdAsync(request.Id, x => x.IsDefault);

            if (isDefault)
            {
                return new ResponseModel("This status group cannot be deleted");
            }

            return await base.Handle(request, cancellationToken);
        }
    }
}
