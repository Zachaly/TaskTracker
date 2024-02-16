using System.Reflection;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Api.Infrastructure
{
    public static class SpacePermissionTypes
    {
        public const string CanAddUsers = "CanAddUsers";
        public const string CanRemoveUsers = "CanAddUsers";
        public const string CanChangePermissions = "CanAddUsers";
        public const string CanModifyLists = "CanAddUsers";
        public const string CanRemoveLists = "CanAddUsers";
        public const string CanModifyTasks = "CanAddUsers";
        public const string CanRemoveTasks = "CanAddUsers";
        public const string CanAssignTaskUsers = "CanAddUsers";
        public const string CanModifyStatusGroups = "CanAddUsers";
        public const string CanModifySpace = "CanAddUsers";
        public const string Owner = "Owner";
    }

    public class SpacePermissionRequiredAttribute : Attribute
    {
        public string Permission { get; set; }

        public SpacePermissionRequiredAttribute(string permission)
        {
            Permission = permission;
        }
    }

    public class SpacePermissionsMiddleware : IMiddleware
    {
        private readonly IUserSpaceRepository _spaceRepository;
        private readonly ISpaceUserPermissionsRepository _permissionsRepository;
        private readonly ITokenService _tokenService;
        private static IEnumerable<PropertyInfo> _availablePermissions = typeof(SpaceUserPermissions).GetProperties()
            .Where(p => p.PropertyType == typeof(bool));

        public SpacePermissionsMiddleware(IUserSpaceRepository spaceRepository, ISpaceUserPermissionsRepository permissionsRepository,
            ITokenService tokenService)
        {
            _spaceRepository = spaceRepository;
            _permissionsRepository = permissionsRepository;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var attribute = context.GetEndpoint()?.Metadata.GetMetadata<SpacePermissionRequiredAttribute>();

            if(attribute is null)
            {
                await next(context);
                return;
            }

            var spaceIdHeader = context.Request.Headers["SpaceId"];

            var parsed = long.TryParse(spaceIdHeader, out var spaceId);

            if(!parsed)
            {
                context.Response.StatusCode = 400;
                return;
            }

            var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];
            var userId = await _tokenService.GetUserIdFromAccessTokenAsync(token);

            var space = await _spaceRepository.GetByIdAsync(spaceId);

            if (space is null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            if(space.Owner.Id == userId)
            {
                await next(context);
                return;
            }

            var permissions = await _permissionsRepository.GetBySpaceIdAndUserIdAsync(spaceId, userId);

            if(permissions is null)
            {
                context.Response.StatusCode = 403;
                return;
            }

            if(_availablePermissions.Any(p => p.Name == attribute.Permission))
            {
                var hasPermission = (bool)_availablePermissions.First(p => p.Name == attribute.Permission).GetValue(permissions)!;

                if(!hasPermission)
                {
                    context.Response.StatusCode = 403;
                    return;
                }
            } 
            else
            {
                context.Response.StatusCode = 403;
                return;
            }

            await next(context);
        }
    }
}
