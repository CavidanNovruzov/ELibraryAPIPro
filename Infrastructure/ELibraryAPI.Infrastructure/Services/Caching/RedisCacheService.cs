using ELibraryAPI.Application.Abstractions.Services.Caching;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

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

    private IDatabase Database => _connectionProvider.Connection.GetDatabase();

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default) where T : class
    {
        try
        {
            var value = await Database.StringGetAsync(key, CommandFlags.DemandReplica);
            if (value.IsNullOrEmpty) return null;

            await Database.KeyExpireAsync(key, TimeSpan.FromMinutes(5), CommandFlags.DemandReplica);

            return JsonSerializer.Deserialize<T>((byte[])value!, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis GET error occurred for key: {Key}", key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken ct = default) where T : class
    {
        try
        {
            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
            TimeSpan expiry = absoluteExpiration ?? slidingExpiration ?? TimeSpan.FromMinutes(5);

            await Database.StringSetAsync(key, jsonBytes, expiry, flags: CommandFlags.DemandMaster);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis SET error occurred for key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        try
        {
            await Database.KeyDeleteAsync(key, CommandFlags.DemandMaster);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis REMOVE error occurred for key: {Key}", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        try
        {
            var endpoints = _connectionProvider.Connection.GetEndPoints();
            var masterEndpoint = endpoints.FirstOrDefault(e => !_connectionProvider.Connection.GetServer(e).IsReplica);

            if (masterEndpoint != null)
            {
                var server = _connectionProvider.Connection.GetServer(masterEndpoint);
                var keys = server.Keys(pattern: $"{prefix}*").ToArray();

                if (keys.Length > 0)
                {
                    await Database.KeyDeleteAsync(keys, CommandFlags.DemandMaster);
                    _logger.LogInformation("Successfully deleted {Count} cache keys with prefix: '{Prefix}'", keys.Length, prefix);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis RemoveByPrefix error occurred for prefix: {Prefix}", prefix);
        }
    }
}