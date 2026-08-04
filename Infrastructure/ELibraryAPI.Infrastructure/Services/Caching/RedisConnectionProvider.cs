
using ELibraryAPI.Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ELibraryAPI.Infrastructure.Services.Caching;

public sealed class RedisConnectionProvider : IRedisConnectionProvider
{
    private readonly RedisSettings _settings;
    private readonly ILogger<RedisConnectionProvider> _logger;
    private IConnectionMultiplexer? _connection;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RedisConnectionProvider(IOptions<RedisSettings> options, ILogger<RedisConnectionProvider> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<IConnectionMultiplexer> GetConnectionAsync()
    {
        if (_connection is { IsConnected: true })
            return _connection;

        await _lock.WaitAsync();
        try
        {
            if (_connection is { IsConnected: true })
                return _connection;

            _logger.LogInformation("Initializing Native Redis Sentinel connection asynchronously...");

            var sentinelEndpoints = _settings.SentinelEndpoints;
            if (sentinelEndpoints == null || sentinelEndpoints.Count == 0)
                throw new ArgumentNullException(nameof(_settings.SentinelEndpoints), "Redis Sentinel endpoints are missing.");

            var masterName = _settings.MasterName ?? "mymaster";

            var options = new ConfigurationOptions
            {
                ServiceName = masterName,
                AbortOnConnectFail = false,
                ConnectTimeout = 10000,
                SyncTimeout = 10000,
                AllowAdmin = true
            };

            if (!string.IsNullOrWhiteSpace(_settings.Password))
            {
                options.Password = _settings.Password;
            }

            foreach (var endpoint in sentinelEndpoints)
            {
                options.EndPoints.Add(endpoint);
                _logger.LogInformation("Added Sentinel Endpoint: {Endpoint}", endpoint);
            }

            _connection = await ConnectionMultiplexer.ConnectAsync(options);

            _logger.LogInformation("Redis Multiplexer configured via Sentinel successfully.");

            return _connection;
        }
        finally
        {
            _lock.Release();
        }
    }
}