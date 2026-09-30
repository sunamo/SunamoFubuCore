namespace SunamoFubuCore;

[Description("Delegates to a BindingAttribute class on the property for one-off binding policies")]
public class AttributePropertyBinder : IPropertyBinder
{
    public bool Matches(PropertyInfo property)
    {
        return property.HasAttribute<BindingAttribute>();
    }

    public void Bind(PropertyInfo property, IBindingContext context)
    {
        property.ForAttribute<BindingAttribute>(att =>
        {
            att.Bind(property, context);
        });
    }
}
