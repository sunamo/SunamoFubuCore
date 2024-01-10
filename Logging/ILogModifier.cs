namespace SunamoFubuCore.Logging;

public interface ILogModifier
{
    bool Matches(Type logType);
    void Modify(object log);
}
