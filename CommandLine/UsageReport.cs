namespace SunamoFubuCore;

public class CommandLineApplicationReport
{
    private readonly IList<CommandReport> _commands = new List<CommandReport>();

    public string ApplicationName { get; set; }

    public CommandReport[] Commands
    {
        get => _commands.ToArray();
        set
        {
            _commands.Clear();
            _commands.AddRange(value);
        }
    }
}

public class CommandReport
{
    private readonly IList<ArgumentReport> _arguments = new List<ArgumentReport>();
    private readonly IList<FlagReport> _flags = new List<FlagReport>();
    private readonly IList<UsageReport> _usages = new List<UsageReport>();
    public string Name { get; set; }
    public string Description { get; set; }

    public ArgumentReport[] Arguments
    {
        get => _arguments.ToArray();
        set
        {
            _arguments.Clear();
            _arguments.AddRange(value);
        }
    }

    public FlagReport[] Flags
    {
        get => _flags.ToArray();
        set
        {
            _flags.Clear();
            _flags.AddRange(value);
        }
    }

    public UsageReport[] Usages
    {
        get => _usages.ToArray();
        set
        {
            _usages.Clear();
            _usages.AddRange(value);
        }
    }
}

public class UsageReport
{
    public string Description { get; set; }
    public string Usage { get; set; }
}

public class ArgumentReport
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class FlagReport
{
    public FlagReport()
    {
    }

    public FlagReport(ITokenHandler token)
    {
        UsageDescription = token.ToUsageDescription();
        Description = token.Description;
    }

    public string UsageDescription { get; set; }
    public string Description { get; set; }
}
