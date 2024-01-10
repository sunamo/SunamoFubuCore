namespace SunamoFubuCore.CommandLine;

public interface ICommandCreator
{
    IFubuCommand Create(Type commandType);
}
