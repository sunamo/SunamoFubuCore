namespace SunamoFubuCore.Binding;

[AttributeUsage(AttributeTargets.Property)]
public abstract class BindingAttribute : Attribute
{
    public abstract void Bind(PropertyInfo property, IBindingContext context);
}
