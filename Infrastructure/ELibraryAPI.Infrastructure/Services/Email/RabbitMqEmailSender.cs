using ELibraryAPI.Application.Abstractions.Services.Email;
using ELibraryAPI.Application.Shared.Models;
using MassTransit;



namespace ELibraryAPI.Infrastructure.Services.Email;

public class RabbitMqEmailSender : IEmailSender
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RabbitMqEmailSender(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody, string? plainBody = null, CancellationToken ct = default)
    {
        await _publishEndpoint.Publish(new EmailMessage(to, subject, htmlBody, plainBody), ct);
    }
}
