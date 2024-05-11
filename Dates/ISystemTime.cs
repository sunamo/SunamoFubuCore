namespace SunamoFubuCore;

public interface ISystemTime
{
    DateTime UtcNow();

    LocalTime LocalTime();
}
