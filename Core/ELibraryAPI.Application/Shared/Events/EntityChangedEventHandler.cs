
using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Shared.Caching;
using MediatR;

namespace ELibraryAPI.Application.Shared.Events;

public sealed class EntityChangedEventHandler : INotificationHandler<EntityChangedEvent>
{
    private readonly ICacheService _cacheService;

    public EntityChangedEventHandler(ICacheService cacheService)
        => _cacheService = cacheService;

    public async Task Handle(EntityChangedEvent notification, CancellationToken ct)
    {
        var listKeyPrefix = $"{notification.EntityName}s:list";
        await _cacheService.RemoveByPrefixAsync(listKeyPrefix, ct);

        if (notification.EntityId.HasValue)
        {
            var detailKey = CacheKeyHelper.Create(notification.EntityName, "id", notification.EntityId.Value);
            await _cacheService.RemoveAsync(detailKey, ct);
        }
    }
}