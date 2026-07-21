using StackExchange.Redis;

namespace ELibraryAPI.Infrastructure.Services.Caching; 

public interface IRedisConnectionProvider
{
    IConnectionMultiplexer Connection { get; }
}