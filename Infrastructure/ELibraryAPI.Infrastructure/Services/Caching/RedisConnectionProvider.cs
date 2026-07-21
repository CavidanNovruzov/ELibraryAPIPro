using ELibraryAPI.Application.Options; 
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; 
using StackExchange.Redis;
using System.Net;

namespace ELibraryAPI.Infrastructure.Services.Caching;

public sealed class RedisConnectionProvider : IRedisConnectionProvider
{
    private readonly RedisSettings _settings;
    private readonly ILogger<RedisConnectionProvider> _logger;
    private readonly Lazy<IConnectionMultiplexer> _connection;

    public RedisConnectionProvider(IOptions<RedisSettings> options, ILogger<RedisConnectionProvider> logger)
    {
        _settings = options.Value; 
        _logger = logger;
        _connection = new Lazy<IConnectionMultiplexer>(CreateConnection, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public IConnectionMultiplexer Connection => _connection.Value;

    private IConnectionMultiplexer CreateConnection()
    {
        _logger.LogInformation("Initializing Redis Sentinel connection...");

        var sentinelEndpoints = _settings.SentinelEndpoints;

        if (sentinelEndpoints == null || sentinelEndpoints.Count == 0)
            throw new ArgumentNullException("SentinelEndpoints", "Redis Sentinel endpoints were not found in configuration.");

        var masterName = _settings.MasterName ?? "mymaster";
        var isLocalDockerDev = _settings.IsLocalDockerDev;

        var sentinelOptions = new ConfigurationOptions
        {
            ServiceName = masterName,
            CommandMap = CommandMap.Sentinel,
            DefaultVersion = new Version(7, 0),
            AbortOnConnectFail = false
        };

        foreach (var endpoint in sentinelEndpoints)
        {
            sentinelOptions.EndPoints.Add(endpoint);
        }

        using var sentinelConnection = ConnectionMultiplexer.SentinelConnect(sentinelOptions);
        EndPoint? masterEndPoint = null;

        foreach (var endpoint in sentinelConnection.GetEndPoints())
        {
            var server = sentinelConnection.GetServer(endpoint);
            if (!server.IsConnected) continue;

            try
            {
                masterEndPoint = server.SentinelGetMasterAddressByName(masterName);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to resolve Redis Master address from Sentinel on endpoint: {Endpoint}", endpoint);
            }
        }

        if (masterEndPoint == null)
        {
            throw new RedisConnectionException(ConnectionFailureType.UnableToResolvePhysicalConnection,
                "No active Redis Master could be resolved via Sentinel.");
        }

        string connectionString = masterEndPoint.ToString()!;

        if (isLocalDockerDev)
        {
            if (_settings.IPTranslations != null && _settings.IPTranslations.TryGetValue(connectionString, out var translatedAddress))
            {
                connectionString = translatedAddress;
                _logger.LogInformation("Local Docker IP translated successfully -> {Conn}", connectionString);
            }
        }

        var masterOptions = ConfigurationOptions.Parse(connectionString);
        masterOptions.AbortOnConnectFail = false;
        masterOptions.KeepAlive = 60;

        _logger.LogInformation("Redis Master connection successfully established: {Conn}", connectionString);
        return ConnectionMultiplexer.Connect(masterOptions);
    }
}