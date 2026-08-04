namespace ELibraryAPI.Application.Abstractions.Services;

public interface IEventBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}