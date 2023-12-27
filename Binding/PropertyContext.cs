namespace SunamoFubuCore.Binding;

public class PropertyContext : IPropertyContext
{
    private readonly IBindingContext _parent;
    private readonly IServiceLocator _services;
    private readonly Lazy<BindingValue> _value;

    public PropertyContext(IBindingContext parent, IServiceLocator services, PropertyInfo property)
    {
        _parent = parent;
        _services = services;
        Property = property;

        _value = new Lazy<BindingValue>(() => _parent.Data.RawValue(Property.Name));
    }

    string IConversionRequest.Text => RawValueFromRequest.RawValue as string;

    T IConversionRequest.Get<T>()
    {
        return _parent.Service<T>();
    }

    IConversionRequest IConversionRequest.AnotherRequest(string text)
    {
        return new ConversionRequest(text, Service);
    }

    public BindingValue RawValueFromRequest => _value.Value;

    public PropertyInfo Property { get; }

    public object Object => _parent.Object;

    public T Service<T>()
    {
        return _parent.Service<T>();
    }

    public object Service(Type typeToFind)
    {
        return _services.GetInstance(typeToFind);
    }

    T IPropertyContext.ValueAs<T>()
    {
        return _parent.Data.ValueAs<T>(Property.Name);
    }

    bool IPropertyContext.ValueAs<T>(Action<T> continuation)
    {
        return _parent.Data.ValueAs(Property.Name, continuation);
    }

    public IBindingLogger Logger => _parent.Logger;

    public IContextValues Data => _parent.Data;

    public void SetPropertyValue(object value)
    {
        Property.SetValue(Object, value, null);
    }

    public object GetPropertyValue()
    {
        return Property.GetValue(Object, null);
    }
}
