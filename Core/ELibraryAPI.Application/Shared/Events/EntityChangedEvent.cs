
using MediatR;

namespace ELibraryAPI.Application.Shared.Events;

public record EntityChangedEvent(string EntityName, Guid? EntityId = null) : INotification;