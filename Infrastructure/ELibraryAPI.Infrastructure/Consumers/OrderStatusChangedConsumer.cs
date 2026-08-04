using ELibraryAPI.Application.Abstractions.Services.Email;
using ELibraryAPI.Application.Shared.Models;
using ELibraryAPI.Application.UnitOfWork;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ELibraryAPI.Infrastructure.Consumers;


public sealed class OrderStatusChangedConsumer : IConsumer<OrderStatusChangedMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<OrderStatusChangedConsumer> _logger;

    public OrderStatusChangedConsumer(
        IUnitOfWork unitOfWork,
        IEmailSender emailSender,
        ILogger<OrderStatusChangedConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedMessage> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Order status changed message received. OrderId: {OrderId}, NewStatus: {NewStatus}",
            message.OrderId, message.NewStatusName);

        var order = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: false)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == message.OrderId, context.CancellationToken);

        if (order?.User?.Email is null)
        {
            _logger.LogWarning("Order or user email not found for OrderId: {OrderId}", message.OrderId);
            return;
        }

        await _emailSender.SendEmailAsync(
            to: order.User.Email,
            subject: $"Sifariş statusu yeniləndi — #{order.OrderNumber}",
            htmlBody: $"<p>Hörmətli {order.User.FirstName}, sifarişiniz (#{order.OrderNumber}) " +
                      $"yeni status alıb: <b>{message.NewStatusName}</b>.</p>",
            plainBody: null,
            ct: context.CancellationToken);
    }
}
