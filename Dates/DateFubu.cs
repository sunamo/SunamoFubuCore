namespace SunamoFubuCore;

public class DateFubu
{
    public const string TimeFormat = "ddMMyyyy";
    private DateTime _date;

    // This *has* to be here for serialization
    public DateFubu()
    {
    }

    public DateFubu(DateTime date)
    : this(date.ToString(TimeFormat))
    {
    }

    public DateFubu(int month, int day, int year)
    {
        _date = new DateTime(year, month, day);
    }

    public DateFubu(string ddmmyyyy)
    {
        _date = DateTime.ParseExact(ddmmyyyy, TimeFormat, null);
    }

    public DateTime Day
    {
        get => _date;
        set => _date = value;
    }

    public DateFubu NextDay()
    {
        return new DateFubu(_date.AddDays(1));
    }

    public bool Equals(DateFubu other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return other._date.Equals(_date);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(DateFubu)) return false;
        return Equals((DateFubu)obj);
    }

    public static bool operator ==(DateFubu left, DateFubu right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DateFubu left, DateFubu right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return _date.GetHashCode();
    }

    public override string ToString()
    {
        return _date.ToString(TimeFormat);
    }

    public DateFubu AddDays(int daysFromNow)
    {
        return new DateFubu(_date.AddDays(daysFromNow));
    }

    public DateTime AtTime(TimeSpan time)
    {
        return _date.Date.Add(time);
    }

    public DateTime AtTime(string mmhh)
    {
        return _date.Date.Add(TimeSpanConverterFubu.GetTimeSpan(mmhh));
    }

    public static DateFubu Today()
    {
        return new DateFubu(DateTime.Today);
    }
}
