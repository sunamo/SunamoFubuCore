namespace SunamoFubuCore;

/// <summary>
///     Marker interface for any type of object that is recorded by ILogger.
///     Using this interface is not mandatory, but it will help with diagnostic
///     UI filters
/// </summary>
public abstract class LogRecord : LogTopic
{
    protected LogRecord()
    {
        Id = Guid.NewGuid();
    }

    public DateTime Time { get; set; }
    public Guid Id { get; private set; }
}
