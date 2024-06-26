namespace SunamoFubuCore;



public static class DateTimeExtensionsFubu
{
    public static DateFubu ToDate(this DateTime time)
    {
        return new DateFubu(time);
    }

    public static DateFubu FirstDayOfMonth(this DateTime time)
    {
        return new DateTime(time.Year, time.Month, 1).ToDate();
    }

    public static DateFubu LastDayOfMonth(this DateTime time)
    {
        return new DateTime(time.Year, time.Month, 1).AddMonths(1).AddDays(-1).ToDate();
    }

    /// <summary>
    ///     The time in UTC that the day started in the given time zone for a specific UTC time
    /// </summary>
    /// <param name="utcTime">A point in time, specified in UTC</param>
    /// <param name="timezone">The time zone that determines when the day started</param>
    /// <returns></returns>
    public static DateTime StartOfTimeZoneDayInUtc(this DateTime utcTime, TimeZoneInfo timezone)
    {
        var startOfDayInGivenTimeZone = utcTime.ToLocalTime(timezone).Date;
        return ToUniversalTime(startOfDayInGivenTimeZone, timezone);
    }

    /// <summary>
    ///     Converts a UTC time to a time in the given time zone
    /// </summary>
    /// <param name="targetTimeZone">The time zone to convert to</param>
    /// <param name="utcTime">The UTC time</param>
    /// <returns></returns>
    public static DateTime ToLocalTime(this DateTime utcTime, TimeZoneInfo targetTimeZone)
    {
        if (utcTime.Kind == DateTimeKind.Local) return utcTime;
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, targetTimeZone);
    }

    /// <summary>
    ///     Converts a UTC time to a time in the given time zone
    /// </summary>
    /// <param name="targetTimeZone">The time zone to convert to</param>
    /// <param name="utcTime">The UTC time</param>
    /// <returns></returns>
    public static DateTime ToLocalTime(this DateTime? utcTime, TimeZoneInfo targetTimeZone)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime.Value, targetTimeZone);
    }

    /// <summary>
    ///     Converts a local time to a UTC time
    /// </summary>
    /// <param name="sourceTimeZone">The time zone of the local time</param>
    /// <param name="localTime">The local time</param>
    /// <returns></returns>
    public static DateTime ToUniversalTime(this DateTime localTime, TimeZoneInfo sourceTimeZone)
    {
        if (localTime.Kind == DateTimeKind.Utc) return localTime;
        if (localTime.Kind == DateTimeKind.Local)
            return TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZoneInfo.Local);

        return TimeZoneInfo.ConvertTimeToUtc(localTime, sourceTimeZone);
    }

    public static LocalTime ToLocal(this DateTime localTime, TimeZoneInfo timeZone = null)
    {
        timeZone = timeZone ?? TimeZoneInfo.Local;

        return new LocalTime(localTime.ToUniversalTime(timeZone), timeZone);
    }
}
