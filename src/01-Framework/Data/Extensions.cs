using App.Framework.Data.Interceptors;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
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

    
    public static IServiceCollection AddDbContextServices<T>(this IServiceCollection services, string connectionString)
        where T:AppDbContext
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        services.AddDbContext<T>((sp, cfg) => 
        {
            cfg.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());   
            cfg.UseSqlServer(connectionString);
        });
        return services;
    }
}
