
using ELibraryAPI.Application.Shared.Models;
using ELibraryAPI.Infrastructure.Services.Email;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ELibraryAPI.Infrastructure.Consumers;

public class SendEmailConsumer : IConsumer<EmailMessage>
{
    private readonly SmtpEmailSender _smtpEmailSender;
    private readonly ILogger<SendEmailConsumer> _logger;

    public SendEmailConsumer(SmtpEmailSender smtpEmailSender, ILogger<SendEmailConsumer> logger)
    {
        _smtpEmailSender = smtpEmailSender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmailMessage> context)
    {
        var message = context.Message;

        _logger.LogInformation("An email message has been received from RabbitMQ. Recipient: {To}", message.To);

        await _smtpEmailSender.SendEmailAsync(
            message.To,
            message.Subject,
            message.HtmlBody,
            message.PlainBody,
            context.CancellationToken
        );
    }
}