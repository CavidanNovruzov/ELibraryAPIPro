using System.Threading.Channels;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Shared.Models;

namespace ELibraryAPI.Infrastructure.Services.Auth;

public class QueuedEmailSender : IEmailSender
{
    private readonly ChannelWriter<EmailMessage> _writer;

    public QueuedEmailSender(Channel<EmailMessage> channel) => _writer = channel.Writer;

    public async Task SendEmailAsync(string to, string subject, string htmlBody, string? plainBody = null, CancellationToken ct = default)
    {
        await _writer.WriteAsync(new EmailMessage(to, subject, htmlBody, plainBody), ct);
    }
}
