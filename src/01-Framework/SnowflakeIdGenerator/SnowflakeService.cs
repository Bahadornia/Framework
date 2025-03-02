using SnowflakeGenerator;

namespace App.Framework.SnowflakeIdGenerator;

internal class SnowflakeService : ISnowflakeService
{
    public long CreateId()
    {
        Settings settings = new()
        {
            MachineID = 1,
            CustomEpoch = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };

        Snowflake snowflake = new Snowflake(settings);
        return snowflake.NextID();
    }
}
