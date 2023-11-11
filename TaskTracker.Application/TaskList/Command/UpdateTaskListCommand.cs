using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateTaskListCommand : UpdateTaskListRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateTaskListHandler : IRequestHandler<UpdateTaskListCommand, ResponseModel>
    {
        private readonly ITaskListRepository _taskListRepository;
        private readonly IValidator<UpdateTaskListCommand> _validator;

        public UpdateTaskListHandler(ITaskListRepository taskListRepository, IValidator<UpdateTaskListCommand> validator)
        {
            _taskListRepository = taskListRepository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateTaskListCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var list = await _taskListRepository.GetByIdAsync(request.Id, l => l);

            if(list is null)
            {
                return new ResponseModel("Entity not found");
            }

            list.Description = request.Description ?? list.Description;
            list.Title = request.Title ?? list.Title;
            list.Color = request.Color ?? list.Color;

            await _taskListRepository.UpdateAsync(list);

            return new ResponseModel();
        }
    }
}
