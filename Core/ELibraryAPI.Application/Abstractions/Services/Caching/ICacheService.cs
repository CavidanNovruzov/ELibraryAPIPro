namespace ELibraryAPI.Application.Abstractions.Services.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        where T : class;

    /// <summary>
    /// Datanı həm absolute, həm də sliding expiration dəstəyi ilə keşə yazır.
    /// </summary>
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null,
        CancellationToken ct = default)
        where T : class;

    Task RemoveAsync(string key, CancellationToken ct = default);

    Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default);
}