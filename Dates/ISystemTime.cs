namespace SunamoFubuCore.Dates;

public interface ISystemTime
{
    DateTime UtcNow();

    LocalTime LocalTime();
}
