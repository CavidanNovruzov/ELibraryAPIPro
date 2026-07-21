namespace ELibraryAPI.Application.Shared.Caching;

public static class CacheKeyHelper
{
    public static string Create(string entity, string action, params object?[] parameters)
    {
        var formattedParams = parameters.Select(p => p?.ToString() ?? "all");
        return $"{entity}:{action}:{string.Join(":", formattedParams)}";
    }
}
