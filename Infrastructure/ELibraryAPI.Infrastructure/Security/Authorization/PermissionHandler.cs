using Microsoft.AspNetCore.Authorization;


namespace ELibraryAPI.Infrastructure.Security.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissions = context.User.FindAll("permission").Select(x => x.Value);

        if (permissions.Any(p => p == requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}