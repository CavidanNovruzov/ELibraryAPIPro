using ELibraryAPI.Application.Abstractions.Repositories;
using ELibraryAPI.Application.Abstractions.Repositories.Auth;
using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Abstractions.Services.Auth;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete.Auth;
using ELibraryAPI.Infrastructure.Services;
using ELibraryAPI.Persistance.Concrete.Repositories;
using ELibraryAPI.Persistance.Concrete.Repositories.Auth;
using ELibraryAPI.Persistence.Contexts;
using ELibraryAPI.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ELibraryAPI.Persistance;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IHostEnvironment env, IConfiguration configuration)
    {
        services.AddDbContext<ELibraryDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            
            if (env.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<ELibraryDbContext>()
        .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));

        services.AddScoped<IRefreshTokenReadRepository, RefreshTokenReadRepository>();
        services.AddScoped<IRefreshTokenWriteRepository, RefreshTokenWriteRepository>();
        services.AddScoped<IPermissionReadRepository, PermissionReadRepository>();
        services.AddScoped<IPermissionWriteRepository, PermissionWriteRepository>();
        services.AddScoped<IRolePermissionReadRepository, RolePermissionReadRepository>();
        services.AddScoped<IRolePermissionWriteRepository, RolePermissionWriteRepository>();
        services.AddScoped<IUserPermissionReadRepository, UserPermissionReadRepository>();
        services.AddScoped<IUserPermissionWriteRepository, UserPermissionWriteRepository>();

        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHealthChecks()
        .AddDbContextCheck<ELibraryDbContext>(name: "Database (SQL Server)");
        return services;
    }
}

