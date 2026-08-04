using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ELibraryAPI.API.Swagger;

public class SwaggerPermissionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authAttributes = context.MethodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>();

        if (authAttributes != null && authAttributes.Any())
        {
            var permissions = authAttributes
                .Where(a => !string.IsNullOrEmpty(a.Policy))
                .Select(a => a.Policy)
                .ToList();

            if (permissions.Any())
            {
                operation.Description += $"<br/><strong> Lazım olan İcazələr (Permissions):</strong> <code>{string.Join(", ", permissions)}</code>";
            }
        }
    }
}