using FluentValidation;
using MediatR;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.Response;
using TaskTracker.Model.TaskStatusGroup.Request;
using TaskTracker.Model.User.Request;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class RegisterCommand : RegisterRequest, IRequest<CreatedResponseModel>
    {
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CreatedResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly IValidator<RegisterCommand> _validator;
        private readonly IUserTaskStatusFactory _userTaskStatusFactory;
        private readonly IUserFactory _userFactory;
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;
        private readonly ITaskStatusGroupRepository _taskStatusGroupRepository;
        private readonly ITaskStatusGroupFactory _taskStatusGroupFactory;

        public RegisterCommandHandler(IUserRepository userRepository, IHashService hashService,
            IUserFactory userFactory, IValidator<RegisterCommand> validator,
            IUserTaskStatusFactory userTaskStatusFactory,
            IUserTaskStatusRepository userTaskStatusRepository,
            ITaskStatusGroupFactory taskStatusGroupFactory,
            ITaskStatusGroupRepository taskStatusGroupRepository)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _validator = validator;
            _userTaskStatusFactory = userTaskStatusFactory;
            _userFactory = userFactory;
            _userTaskStatusRepository = userTaskStatusRepository;
            _taskStatusGroupRepository = taskStatusGroupRepository;
            _taskStatusGroupFactory = taskStatusGroupFactory;
        }

        public async Task<CreatedResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if((await _userRepository.GetByEmailAsync(request.Email)) is not null)
            {
                return new CreatedResponseModel("Email already taken");
            }

            var validation = await _validator.ValidateAsync(request);

            if(!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var user = _userFactory.Create(request, await _hashService.HashStringAsync(request.Password));

            await _userRepository.AddAsync(user);

            await CreateDefaultStatusGroup(user.Id);

            return new CreatedResponseModel(user.Id);
        }

        private async Task CreateDefaultStatusGroup(long userId)
        {
            var statusGroup = _taskStatusGroupFactory.Create(new AddTaskStatusGroupRequest
            {
                UserId = userId,
                Name = "Default"
            }, true);

            var statusGroupId = await _taskStatusGroupRepository.AddAsync(statusGroup);

            var backlogStatus = _userTaskStatusFactory.Create(new AddUserTaskStatusRequest
            {
                Color = "#",
                GroupId = statusGroupId,
                Index = 0,
                Name = "Backlog"
            }, true);

            var closedStatus = _userTaskStatusFactory.Create(new AddUserTaskStatusRequest
            {
                Color = "#",
                GroupId = statusGroupId,
                Index = 21,
                Name = "Closed"
            }, true);


            await _userTaskStatusRepository.AddAsync(backlogStatus, closedStatus);
        }
    }
}
