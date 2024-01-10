namespace SunamoFubuCore.Logging;

public interface ILogger
{
    void Debug(string message, params string[] parameters);
    void Info(string message, params string[] parameters);
    void Error(string message, Exception ex);
    void Error(object correlationId, string message, Exception ex);

    void Debug(Func<string> message);
    void Info(Func<string> message);

    void DebugMessage(LogTopic message);
    void InfoMessage(LogTopic message);

    void DebugMessage<T>(Func<T> message) where T : class, LogTopic;
    void InfoMessage<T>(Func<T> message) where T : class, LogTopic;
}
