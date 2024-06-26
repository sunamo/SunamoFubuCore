namespace SunamoFubuCore;



public class Logger : ILogger
{
    private readonly Cache<Type, Action<Func<object>>> _debugMessage = new Cache<Type, Action<Func<object>>>();
    private readonly Lazy<Action<Func<string>>> _debugString;
    private readonly Cache<Type, Action<Func<object>>> _infoMessage = new Cache<Type, Action<Func<object>>>();
    private readonly Lazy<Action<Func<string>>> _infoString;
    private readonly ListenerCollection _listeners;

    public Logger(IEnumerable<ILogListener> listeners, IEnumerable<ILogModifier> modifiers)
    {
        _listeners = new ListenerCollection(listeners, modifiers);
        _debugString = new Lazy<Action<Func<string>>>(() => _listeners.Debug());
        _infoString = new Lazy<Action<Func<string>>>(() => _listeners.Info());

        _debugMessage.OnMissing = type => _listeners.DebugFor(type);
        _infoMessage.OnMissing = type => _listeners.InfoFor(type);
    }

    public void Debug(string message, params string[] parameters)
    {
        Debug(() => message.ToFormat(parameters));
    }

    public void Info(string message, params string[] parameters)
    {
        Info(() => message.ToFormat(parameters));
    }

    public void Error(string message, Exception ex)
    {
        _listeners.Each(x =>
        {
            try
            {
                x.Error(message, ex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
    }

    public void Error(object correlationId, string message, Exception ex)
    {
        _listeners.Each(x =>
        {
            try
            {
                x.Error(correlationId, message, ex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
    }

    public void Debug(Func<string> message)
    {
        _debugString.Value(message);
    }

    public void Info(Func<string> message)
    {
        _infoString.Value(message);
    }

    public void DebugMessage(LogTopic message)
    {
        if (message == null) return;


        _debugMessage[message.GetType()](() => message);
    }

    public void InfoMessage(LogTopic message)
    {
        if (message == null) return;

        _infoMessage[message.GetType()](() => message);
    }

    public void DebugMessage<T>(Func<T> message) where T : class, LogTopic
    {
        _debugMessage[typeof(T)](message);
    }

    public void InfoMessage<T>(Func<T> message) where T : class, LogTopic
    {
        _infoMessage[typeof(T)](message);
    }
}
