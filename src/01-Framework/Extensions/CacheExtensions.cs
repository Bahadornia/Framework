using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace App.Framework.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisSettings> config)
    {
        var newConfig = new RedisSettings();
        config.Invoke(newConfig);
        services.TryAddSingleton<IConnectionMultiplexer>(sp =>
        {
            return ConnectionMultiplexer.Connect(newConfig.BaseUrl);
        });
        return services;
    }
}
