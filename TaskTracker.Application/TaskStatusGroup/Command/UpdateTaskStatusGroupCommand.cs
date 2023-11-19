using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskStatusGroup.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateTaskStatusGroupCommand : UpdateTaskStatusGroupRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateTaskStatusGroupHandler : IRequestHandler<UpdateTaskStatusGroupCommand, ResponseModel>
    {
        private readonly ITaskStatusGroupRepository _taskStatusGroupRepository;
        private readonly IValidator<UpdateTaskStatusGroupCommand> _validator;

        public UpdateTaskStatusGroupHandler(ITaskStatusGroupRepository taskStatusGroupRepository,
            IValidator<UpdateTaskStatusGroupCommand> validator)
        {
            _taskStatusGroupRepository = taskStatusGroupRepository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateTaskStatusGroupCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var group = await _taskStatusGroupRepository.GetByIdAsync(request.Id, x => x);

            if(group is null)
            {
                return new ResponseModel("Entity not found");
            }

            group.Name = request.Name ?? group.Name;

            await _taskStatusGroupRepository.UpdateAsync(group);

            return new ResponseModel();
        }
    }
}
