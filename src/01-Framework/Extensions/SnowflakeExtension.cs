using App.Framework.SnowflakeIdGenerator;
using Microsoft.Extensions.DependencyInjection;
using SnowflakeGenerator;

namespace App.Framework.Extensions;

public static class SnowflakeExtension
{
    public static IServiceCollection AddSnowflakeService(this IServiceCollection services, int generatorId = 1234654897)
    {
        services.AddSingleton<ISnowflakeService, SnowflakeService>();
        return services;
    }
}
