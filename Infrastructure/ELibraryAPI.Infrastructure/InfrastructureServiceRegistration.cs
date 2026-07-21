using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Abstractions.Services.Storage;
using ELibraryAPI.Application.Options;
using ELibraryAPI.Infrastructure.Security.Authorization;
using ELibraryAPI.Infrastructure.Services.Auth;
using ELibraryAPI.Infrastructure.Services.Auth.Token;
using ELibraryAPI.Infrastructure.Services.Caching;
using ELibraryAPI.Infrastructure.Services.Storage;
using HealthChecks.Redis;
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
        services.AddScoped<IEmailSender, SmtpEmailSender>();

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

        services.AddHealthChecks()
        .AddCheck<RedisHealthCheck>("Redis Sentinel");
    }
}