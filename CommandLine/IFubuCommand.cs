namespace SunamoFubuCore.CommandLine;

public interface IFubuCommand
{
    Type InputType { get; }
    UsageGraph Usages { get; }
    bool Execute(object input);
}

public interface IFubuCommand<T> : IFubuCommand
{
}
