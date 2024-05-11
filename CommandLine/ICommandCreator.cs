namespace SunamoFubuCore;

public interface ICommandCreator
{
    IFubuCommand Create(Type commandType);
}
