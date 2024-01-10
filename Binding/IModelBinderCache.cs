namespace SunamoFubuCore.Binding;

public interface IModelBinderCache
{
    IModelBinder BinderFor(Type modelType);
}
