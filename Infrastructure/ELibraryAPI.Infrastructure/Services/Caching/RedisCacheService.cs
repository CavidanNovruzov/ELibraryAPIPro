using System.Text.Json;
using ELibraryAPI.Application.Abstractions.Services.Caching;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ELibraryAPI.Infrastructure.Services.Caching;

public sealed class RedisCacheService : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    private readonly IRedisConnectionProvider _connectionProvider;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IRedisConnectionProvider connectionProvider, ILogger<RedisCacheService> logger)
    {
        _connectionProvider = connectionProvider;
        _logger = logger;
    }

    private async Task<IDatabase?> GetDatabaseAsync()
    {
        try
        {
            var connection = await _connectionProvider.GetConnectionAsync();
            return connection?.GetDatabase();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get Redis database connection.");
            return null;
        }
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default) where T : class
    {
        try
        {
            var db = await GetDatabaseAsync();
            if (db == null) return null;

            var value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<T>((byte[])value!, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis GET failed, falling back to the DB. Key: {Key}", key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken ct = default) where T : class
    {
        try
        {
            var db = await GetDatabaseAsync();
            if (db == null) return;

            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
            TimeSpan expiry = absoluteExpiration ?? slidingExpiration ?? TimeSpan.FromMinutes(5);

            await db.StringSetAsync(key, jsonBytes, expiry);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis SET failed, could not write to cache. Key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        try
        {
            var db = await GetDatabaseAsync();
            if (db == null) return;

            await db.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis REMOVE failed, could not delete key. Key: {Key}", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        try
        {
            var connection = await _connectionProvider.GetConnectionAsync();
            if (connection == null || !connection.IsConnected) return;

            var endpoints = connection.GetEndPoints();
            var masterEndpoint = endpoints.FirstOrDefault(e => !connection.GetServer(e).IsReplica);

            if (masterEndpoint != null)
            {
                var server = connection.GetServer(masterEndpoint);
                var keys = server.Keys(pattern: $"{prefix}*").ToArray();

                if (keys.Length > 0)
                {
                    var db = connection.GetDatabase();
                    await db.KeyDeleteAsync(keys, CommandFlags.DemandMaster);
                    _logger.LogInformation("Successfully deleted {Count} cache keys with prefix: '{Prefix}'", keys.Length, prefix);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis RemoveByPrefix failed. Prefix: {Prefix}", prefix);
        }
    }
}