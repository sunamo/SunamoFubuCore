namespace SunamoFubuCore;

public class MachineTimeZoneContext : ITimeZoneContext
{
    public TimeZoneInfo GetTimeZone()
    {
        return TimeZoneInfo.Local;
    }
}
