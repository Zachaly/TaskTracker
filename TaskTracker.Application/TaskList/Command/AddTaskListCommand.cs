using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskList.Request;

namespace TaskTracker.Application.Command
{
    public class AddTaskListCommand : AddTaskListRequest, IRequest<CreatedResponseModel>
    {
    }

    public class AddTaskListHandler : IRequestHandler<AddTaskListCommand, CreatedResponseModel>
    {
        private readonly ITaskListRepository _taskListRepository;
        private readonly ITaskListFactory _taskListFactory;
        private readonly IValidator<AddTaskListCommand> _validator;

        public AddTaskListHandler(ITaskListRepository taskListRepository, ITaskListFactory taskListFactory,
            IValidator<AddTaskListCommand> validator)
        {
            _taskListRepository = taskListRepository;
            _taskListFactory = taskListFactory;
            _validator = validator;
        }

        public async Task<CreatedResponseModel> Handle(AddTaskListCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var list = _taskListFactory.Create(request);

            var id = await _taskListRepository.AddAsync(list);

            return new CreatedResponseModel(id);
        }
    }
}
