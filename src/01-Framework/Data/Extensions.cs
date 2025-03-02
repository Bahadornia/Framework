using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace App.Framework.Data;

public static class Extensions
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        MapsterConfig.RegisterMapsterConfigurations();
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
