namespace SunamoFubuCore.Binding;

public interface IPropertyBinderCache
{
    IPropertyBinder BinderFor(PropertyInfo property);
    IEnumerable<IPropertyBinder> AllPropertyBinders();
}
