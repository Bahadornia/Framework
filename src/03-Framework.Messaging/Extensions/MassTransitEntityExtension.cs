using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using App.Framework.Data;

namespace App.Framework.Messaging.Extensions;

public static class MassTransitEntityExtension
{
    public static ModelBuilder AddMassTransitEntityFrameworkServices(this ModelBuilder modelBuilder)
    {
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
        return modelBuilder;
    }
    public static IServiceCollection AddMassTransitServices<TContext>(this IServiceCollection services, Action<RabbitConfig> config, params Assembly[] assemblies)
        where TContext : AppDbContext
    {
        var rabbitConfig = new RabbitConfig();
        config.Invoke(rabbitConfig);
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            //config.SetInMemorySagaRepositoryProvider();

            config.AddConsumers(assemblies);
            //config.AddSagaStateMachines(assemblies);
            //config.AddSagas(assemblies);
            //config.AddActivities(assemblies);

            config.AddEntityFrameworkOutbox<TContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox();
            });

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(rabbitConfig.BaseUrl), host =>
                {
                    host.Username(rabbitConfig.Username);
                    host.Password(rabbitConfig.Password);
                });
                configurator.ConfigureEndpoints(context);
               
            configurator.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(5)));
            });
            config.AddConfigureEndpointsCallback((context, name, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<TContext>(context);
            });

        });
        return services;
    }
}
