using FluentValidation;
using TaskTracker.Application.Abstraction;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.UserTaskStatus;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class AddUserTaskStatusCommand : AddUserTaskStatusRequest, IAddEntityCommand
    {
    }

    public class AddUserTaskStatusHandler : AddEntityHandler<UserTaskStatus, UserTaskStatusModel,
        AddUserTaskStatusRequest, AddUserTaskStatusCommand>
    {
        public AddUserTaskStatusHandler(IUserTaskStatusRepository userTaskStatusRepository, IUserTaskStatusFactory userTaskStatusFactory,
            IValidator<AddUserTaskStatusCommand> validator) : base(userTaskStatusRepository, userTaskStatusFactory, validator)
        {
        }
    }
}
