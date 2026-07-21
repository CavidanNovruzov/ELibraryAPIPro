using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ELibraryAPI.Application.Shared.Models;

namespace ELibraryAPI.Infrastructure.Services.Auth;

public class EmailBackgroundWorker : BackgroundService
{
    private readonly ChannelReader<EmailMessage> _reader;
    private readonly IServiceProvider _serviceProvider;

    public EmailBackgroundWorker(Channel<EmailMessage> channel, IServiceProvider serviceProvider)
    {
        _reader = channel.Reader;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var email in _reader.ReadAllAsync(stoppingToken))
        {
            using var scope = _serviceProvider.CreateScope();

            var realSender = scope.ServiceProvider.GetRequiredService<SmtpEmailSender>();

            try
            {
                await realSender.SendEmailAsync(email.To, email.Subject, email.HtmlBody, email.PlainBody, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}