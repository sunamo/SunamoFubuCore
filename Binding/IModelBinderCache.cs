namespace SunamoFubuCore;

public interface IModelBinderCache
{
    IModelBinder BinderFor(Type modelType);
}
