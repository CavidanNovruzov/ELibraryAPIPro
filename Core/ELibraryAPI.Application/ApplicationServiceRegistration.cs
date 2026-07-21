using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ELibraryAPI.Application.Behaviors;
using FluentValidation;
using MediatR;

namespace ELibraryAPI.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
       
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

    }
}
