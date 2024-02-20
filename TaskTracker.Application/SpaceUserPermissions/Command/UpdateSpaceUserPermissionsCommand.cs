using FluentValidation;
using MediatR;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;
using TaskTracker.Model.SpaceUserPermissions.Request;

namespace TaskTracker.Application.Command
{
    public class UpdateSpaceUserPermissionsCommand : UpdateSpaceUserPermissionsRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateSpaceUserPermissionsHandler : IRequestHandler<UpdateSpaceUserPermissionsCommand, ResponseModel>
    {
        private readonly ISpaceUserPermissionsRepository _repository;
        private readonly IValidator<UpdateSpaceUserPermissionsCommand> _validator;

        public UpdateSpaceUserPermissionsHandler(ISpaceUserPermissionsRepository repository,
            IValidator<UpdateSpaceUserPermissionsCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ResponseModel> Handle(UpdateSpaceUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if(!validation.IsValid)
            {
                return new ResponseModel(validation.ToDictionary());
            }

            var permissions = await _repository.GetBySpaceIdAndUserIdAsync(request.SpaceId, request.UserId);

            if(permissions is null)
            {
                return new ResponseModel("Entity not found");
            }

            permissions.CanAddUsers = request.CanAddUsers;
            permissions.CanRemoveUsers = request.CanRemoveUsers;
            permissions.CanChangePermissions = request.CanChangePermissions;
            permissions.CanModifyLists = request.CanModifyLists;
            permissions.CanRemoveLists = request.CanRemoveLists;
            permissions.CanModifyTasks = request.CanModifyTasks;
            permissions.CanRemoveTasks = request.CanRemoveTasks;
            permissions.CanAssignTaskUsers = request.CanAssignTaskUsers;
            permissions.CanModifyStatusGroups = request.CanModifyStatusGroups;
            permissions.CanModifySpace = request.CanModifySpace;

            await _repository.UpdateAsync(permissions);

            return new ResponseModel();
        }
    }
}
