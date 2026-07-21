using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;

namespace ELibraryAPI.Infrastructure.Services.Auth;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpOptions _options;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IOptions<SmtpOptions> options, ILogger<SmtpEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(
     string to,
     string subject,
     string htmlBody,
     string? plainBody = null,
     CancellationToken ct = default)
    {
        if (!_options.SendEmails)
        {
            _logger.LogWarning("Email sending is disabled in configuration options.");
            return;
        }

        if (string.IsNullOrWhiteSpace(to))
        {
            _logger.LogError("Email could not be sent: Recipient address is empty.");
            return;
        }

        var message = CreateMimeMessage(to, subject, htmlBody, plainBody);

        using var client = new SmtpClient();

        try
        {
            _logger.LogInformation("Attempting to send email to {To}...", to);

            client.Timeout = 10000;

            SecureSocketOptions secureSocket = _options.SmtpPort switch
            {
                465 => SecureSocketOptions.SslOnConnect,
                587 => SecureSocketOptions.StartTls,
                _ => SecureSocketOptions.Auto
            };

            await client.ConnectAsync(_options.SmtpHost, _options.SmtpPort, secureSocket, ct);

            if (!string.IsNullOrWhiteSpace(_options.UserName))
            {
                var cleanPassword = _options.Password.Replace(" ", "");
                await client.AuthenticateAsync(_options.UserName, cleanPassword, ct);
            }

            await client.SendAsync(message, ct);

            _logger.LogInformation("Email sent successfully to {To}.", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending email to {To}: {Message}", to, ex.Message);
            throw;
        }
        finally
        {
            if (client.IsConnected)
                await client.DisconnectAsync(true, ct);
        }
    }

    private MimeMessage CreateMimeMessage(string to, string subject, string htmlBody, string? plainBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
        message.To.Add(MailboxAddress.Parse(to.Trim()));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = plainBody ?? string.Empty
        };

        message.Body = bodyBuilder.ToMessageBody();
        return message;
    }
}