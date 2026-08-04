
using StackExchange.Redis;

namespace ELibraryAPI.Infrastructure.Services.Caching;

public interface IRedisConnectionProvider
{
    Task<IConnectionMultiplexer> GetConnectionAsync();
}