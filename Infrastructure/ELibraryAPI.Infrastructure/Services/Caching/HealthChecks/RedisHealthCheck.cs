using ELibraryAPI.Application.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace ELibraryAPI.Infrastructure.Services.Caching.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly RedisSettings _redisSettings;
    private readonly IRedisConnectionProvider _connectionProvider;

    public RedisHealthCheck(IOptions<RedisSettings> options, IRedisConnectionProvider connectionProvider)
    {
        _redisSettings = options.Value;
        _connectionProvider = connectionProvider;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            var db = _connectionProvider.Connection.GetDatabase();

            db.Ping();

            return Task.FromResult(HealthCheckResult.Healthy("Redis Sentinel is running."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Redis Sentinel is unreachable.", ex));
        }
    }
}