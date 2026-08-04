using MediatR;

namespace ELibraryAPI.Application.Shared.Events;

/// <summary>
/// Yalnız sifariş İLK dəfə yaradılanda publish olunur.
/// EntityChangedEvent-dən fərqli olaraq, bu event yalnız "order created" business
/// hadisəsini bildirir — keş təmizləmə məqsədilə istifadə olunan generic
/// EntityChangedEvent ilə QARIŞDIRILMAMALIDIR. Beləliklə, sabah kimsə
/// UpdateOrderStatus və ya CancelOrder handler-inə keş təmizləmə üçün
/// EntityChangedEvent("order", ...) əlavə etsə belə, təsdiq emaili
/// TƏKRAR göndərilməyəcək.
/// </summary>
public sealed record OrderCreatedEvent(Guid OrderId) : INotification;
