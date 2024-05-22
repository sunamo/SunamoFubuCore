namespace SunamoFubuCore;

internal static class TimeSpanExtensions
{
    /// <summary>
    ///     Values are 0 to 2359
    /// </summary>
    /// <param name="minutes"></param>
    /// <returns></returns>
    internal static TimeSpan ToTime(this int minutes)
    {
        var text = minutes.ToString().PadLeft(4, '0');
        return text.ToTime();
    }

    internal static TimeSpan ToTime(this string timeString)
    {
        return TimeSpanConverterFubu.GetTimeSpan(timeString);
    }


    internal static TimeSpan Minutes(this int number)
    {
        return new TimeSpan(0, 0, number, 0);
    }

    internal static TimeSpan Hours(this int number)
    {
        return new TimeSpan(0, number, 0, 0);
    }

    internal static TimeSpan Days(this int number)
    {
        return new TimeSpan(number, 0, 0, 0);
    }

    internal static TimeSpan Seconds(this int number)
    {
        return new TimeSpan(0, 0, number);
    }
}
