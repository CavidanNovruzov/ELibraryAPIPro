using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Abstractions.Services.Email;
using ELibraryAPI.Application.Abstractions.Services.Image;
using ELibraryAPI.Application.Abstractions.Services.Payment;
using ELibraryAPI.Application.Abstractions.Services.Storage;
using ELibraryAPI.Application.Options;
using ELibraryAPI.Infrastructure.Consumers;
using ELibraryAPI.Infrastructure.Security.Authorization;
using ELibraryAPI.Infrastructure.Services;
using ELibraryAPI.Infrastructure.Services.Auth;
using ELibraryAPI.Infrastructure.Services.Auth.Token;
using ELibraryAPI.Infrastructure.Services.Caching;
using ELibraryAPI.Infrastructure.Services.Email;
using ELibraryAPI.Infrastructure.Services.HealthChecks;
using ELibraryAPI.Infrastructure.Services.Image;
using ELibraryAPI.Infrastructure.Services.Payment;
using ELibraryAPI.Infrastructure.Services.Storage;
using ELibraryAPI.Persistence.Contexts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELibraryAPI.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IEmailSender, RabbitMqEmailSender>();
        services.AddScoped<SmtpEmailSender>();

        var storageProvider = configuration["Storage:Provider"];
        if (storageProvider == "Azure")
            services.AddSingleton<IStorageService, AzureBlobStorage>();
        else
            services.AddScoped<IStorageService, LocalStorage>();

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionHandler>();

        services.AddSingleton<IRedisConnectionProvider, RedisConnectionProvider>();
        services.AddSingleton<ICacheService, RedisCacheService>();

        services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IEventBus, EventBus>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<SendEmailConsumer>();
            x.AddConsumer<OrderStatusChangedConsumer>();

            x.AddEntityFrameworkOutbox<ELibraryDbContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox();
                o.QueryDelay = TimeSpan.FromSeconds(5);
            });

            var rabbitMqOptions = configuration.GetSection("RabbitMqOptions").Get<RabbitMqOptions>() ?? new RabbitMqOptions();

            x.UsingRabbitMq((context, cfg) =>
            {
                var host = rabbitMqOptions.Host ?? "localhost";
                var username = rabbitMqOptions.Username ?? "guest";
                var password = rabbitMqOptions.Password ?? "guest";

                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                cfg.ConfigureEndpoints(context);
            });
        });

        var defaultConnection = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection string is missing.");

        var rabbitConnectionString = $"amqp://{configuration["RabbitMqOptions:Username"]}:{configuration["RabbitMqOptions:Password"]}@{configuration["RabbitMqOptions:Host"]}:{configuration["RabbitMqOptions:Port"]}";

        services.AddHealthChecks()
            .AddCheck<RedisHealthCheck>("Redis Sentinel")
            .AddSqlServer(
                connectionString: defaultConnection,
                name: "MSSQL Database",
                timeout: TimeSpan.FromSeconds(3),
                tags: new[] { "db", "sql", "mssql" })
            .AddRabbitMQ(
                factory: async sp =>
                {
                    var factory = new RabbitMQ.Client.ConnectionFactory { Uri = new Uri(rabbitConnectionString) };
                    return await factory.CreateConnectionAsync();
                },
                name: "RabbitMQ Broker",
                timeout: TimeSpan.FromSeconds(3),
                tags: new[] { "messaging", "rabbitmq" });
    }
}