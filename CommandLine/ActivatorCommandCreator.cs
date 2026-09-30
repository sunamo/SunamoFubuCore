namespace SunamoFubuCore;



public class ActivatorCommandCreator : ICommandCreator
{
    public IFubuCommand Create(Type commandType)
    {
        return Activator.CreateInstance(commandType).As<IFubuCommand>();
    }
}
