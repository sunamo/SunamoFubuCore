namespace SunamoFubuCore;

//using SunamoCl;


public class CommandExecutor
{
    public CommandExecutor(ICommandFactory factory)
    {
        Factory = factory;
    }

    public CommandExecutor() : this(new CommandFactory())
    {
    }

    public ICommandFactory Factory { get; }

    public static int ExecuteInConsole<T>(string[] args) where T : CommandExecutor, new()
    {
        bool success;

        try
        {
            var executor = new T();
            success = executor.Execute(args);
        }
        catch (CommandFailureException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + e.Message);
            Console.ResetColor();
            return 1;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + ex);
            Console.ResetColor();
            return 1;
        }

        return success ? 0 : 1;
    }

    public bool Execute(string commandLine)
    {
        var run = Factory.BuildRun(commandLine);
        return run.Execute();
    }

    public bool Execute(string[] args)
    {
        var run = Factory.BuildRun(args);
        return run.Execute();
    }
}
