using App.Framework.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Framework.Extensions;

public static class MediatRExtension
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(cfg=>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}
