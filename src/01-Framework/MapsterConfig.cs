using Mapster;
using System.Reflection;

namespace App.Framework;

public class MapsterConfig
{
    public static void RegisterMapsterConfigurations()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        // Apply all mappings from assemblies
        config.Scan(Assembly.GetExecutingAssembly());
    }
}
