namespace SunamoFubuCore;

public interface ICommandFactory
{
    CommandRun BuildRun(string commandLine);
    CommandRun BuildRun(IEnumerable<string> args);
    void RegisterCommands(Assembly assembly);

    IEnumerable<IFubuCommand> BuildAllCommands();
}
