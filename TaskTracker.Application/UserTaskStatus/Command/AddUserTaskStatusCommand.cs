﻿using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.UserTaskStatus.Request;

namespace TaskTracker.Application.Command
{
    public class AddUserTaskStatusCommand : AddUserTaskStatusRequest, IRequest<CreatedResponseModel>
    {
    }

    public class AddUserTaskStatusHandler : IRequestHandler<AddUserTaskStatusCommand, CreatedResponseModel>
    {
        private readonly IUserTaskStatusRepository _userTaskStatusRepository;
        private readonly IUserTaskStatusFactory _userTaskStatusFactory;
        private readonly IValidator<AddUserTaskStatusCommand> _validator;

        public AddUserTaskStatusHandler(IUserTaskStatusRepository userTaskStatusRepository, IUserTaskStatusFactory userTaskStatusFactory,
            IValidator<AddUserTaskStatusCommand> validator)
        {
            _userTaskStatusRepository = userTaskStatusRepository;
            _userTaskStatusFactory = userTaskStatusFactory;
            _validator = validator;
        }

        public async Task<CreatedResponseModel> Handle(AddUserTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return new CreatedResponseModel(validation.ToDictionary());
            }

            var status = _userTaskStatusFactory.Create(request);

            var statusId = await _userTaskStatusRepository.AddAsync(status);

            return new CreatedResponseModel(statusId);
        }
    }
}
