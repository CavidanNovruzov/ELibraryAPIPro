using ELibraryAPI.Application.Options;
using ELibraryAPI.Infrastructure.Services.Caching;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace ELibraryAPI.Infrastructure.Services.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly RedisSettings _redisSettings;
    private readonly IRedisConnectionProvider _connectionProvider;

    public RedisHealthCheck(IOptions<RedisSettings> options, IRedisConnectionProvider connectionProvider)
    {
        _redisSettings = options.Value;
        _connectionProvider = connectionProvider;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            var db = _connectionProvider.Connection.GetDatabase();

            await db.PingAsync();

            return HealthCheckResult.Healthy("Redis Sentinel is running.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis Sentinel is unreachable.", ex);
        }
    }
}