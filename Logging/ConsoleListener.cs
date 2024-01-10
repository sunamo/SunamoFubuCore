namespace SunamoFubuCore.Logging;

/// <summary>
///     Just what it says, provides a LogListener that writes to the
///     Console
/// </summary>
public class ConsoleListener : FilteredListener<ConsoleListener>, ILogListener
{
    public ConsoleListener(Level level) : base(level)
    {
    }

    public void DebugMessage(object message)
    {
        CL.WriteLineO(message);
    }

    public void InfoMessage(object message)
    {
        CL.WriteLineO(message);
    }

    public void Debug(string message)
    {
        CL.WriteLine(message);
    }

    public void Info(string message)
    {
        CL.WriteLine(message);
    }

    public void Error(string message, Exception ex)
    {
        CL.WriteLine(message);
        CL.WriteLine(ex);
    }

    public void Error(object correlationId, string message, Exception ex)
    {
        CL.WriteLineO(correlationId);
        Error(message, ex);
    }

    protected override ConsoleListener thisInstance()
    {
        return this;
    }
}
