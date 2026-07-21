using MediatR;


namespace ELibraryAPI.Application.Shared.Events;

public sealed record ProductBackInStockEvent(Guid ProductId) : INotification;
