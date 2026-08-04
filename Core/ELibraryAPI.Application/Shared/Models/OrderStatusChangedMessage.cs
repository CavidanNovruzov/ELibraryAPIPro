namespace ELibraryAPI.Application.Shared.Models;

/// <summary>
/// RabbitMQ üzərindən göndərilən mesaj müqaviləsi (EmailMessage ilə eyni konvensiya:
/// Shared/Models altında, həm Application həm Infrastructure layihələri tərəfindən
/// referans edilə bilir). ChangeOrderStatusCommandHandler bunu IPublishEndpoint
/// vasitəsilə publish edəcək, OrderStatusChangedConsumer isə Infrastructure
/// layihəsində bunu consume edib email göndərəcək.
/// </summary>
public record OrderStatusChangedMessage(Guid OrderId, string NewStatusName);
