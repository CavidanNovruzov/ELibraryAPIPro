using ELibraryAPI.Application.Abstractions.Services; 
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Order.CreateOrder;

public sealed class SendOrderConfirmationEmail : INotificationHandler<EntityChangedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender; 

    public SendOrderConfirmationEmail(IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    public async Task Handle(EntityChangedEvent notification, CancellationToken ct)
    {
        if (notification.EntityName != "order" || !notification.EntityId.HasValue)
            return;

        var order = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: false)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == notification.EntityId.Value, ct);

        if (order == null || order.User == null || string.IsNullOrWhiteSpace(order.User.Email))
            return;

        string subject = "Sifarişiniz qəbul edildi — ELibrary.az";
        string htmlBody = $@"
            <h3>Hörmətli {order.User.FirstName} {order.User.LastName},</h3>
            <p>Sifarişiniz uğurla tərəfimizdən qəbul olundu.</p>
            <hr/>
            <p><b>Sifariş Nömrəsi:</b> {order.OrderNumber}</p>
            <p><b>Ümumi Məbləğ:</b> {order.TotalAmount} AZN</p>
            <br/>
            <p>Bizi seçdiyiniz üçün təşəkkür edirik!</p>";


        await _emailSender.SendEmailAsync(
            to: order.User.Email,
            subject: subject,
            htmlBody: htmlBody,
            plainBody: null,
            ct: ct);
    }
}