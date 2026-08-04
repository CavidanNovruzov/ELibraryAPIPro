using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ELibraryAPI.API.Swagger;

public sealed class HideCacheablePropertiesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null || !operation.Parameters.Any())
            return;

        var propertiesToHide = new[]
        {
            "CacheKey",
            "AbsoluteExpiration",
            "SlidingExpiration"
        };

        var propertiesToRemove = operation.Parameters
            .Where(p => propertiesToHide.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
            .ToList();

        foreach ( var p in propertiesToRemove) 
            operation.Parameters.Remove(p);
    }
}
