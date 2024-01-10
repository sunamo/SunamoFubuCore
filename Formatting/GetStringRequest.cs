namespace SunamoFubuCore.Formatting;

public class GetStringRequest
{
    private Type _propertyType;

    private GetStringRequest()
    {
    }

    public GetStringRequest(PropertyInfo property, object rawValue) : this(new SingleProperty(property), rawValue,
    null)
    {
    }


    public GetStringRequest(Accessor accessor, object rawValue, IServiceLocator locator)
    {
        Locator = locator;
        if (accessor != null) Property = accessor.InnerProperty;
        RawValue = rawValue;

        setPropertyType();

        setOwnerType(accessor);
    }

    // Yes, I made this internal.  Don't necessarily want it in the public interface,
    // but needs to be "settable"
    public IServiceLocator Locator { get; set; }

    public Type OwnerType { get; set; }

    public Type PropertyType
    {
        get
        {
            if (_propertyType == null && Property != null) return Property.PropertyType;

            return _propertyType;
        }
        set => _propertyType = value;
    }

    public PropertyInfo Property { get; private set; }
    public object RawValue { get; private set; }
    public string Format { get; set; }

    private void setOwnerType(Accessor accessor)
    {
        if (accessor != null)
            OwnerType = accessor.OwnerType;
        else if (Property != null) OwnerType = Property.DeclaringType;
    }

    private void setPropertyType()
    {
        if (Property != null)
            PropertyType = Property.PropertyType;
        else if (RawValue != null) PropertyType = RawValue.GetType();
    }

    public string WithFormat(string format)
    {
        return string.Format(format, RawValue);
    }

    public GetStringRequest GetRequestForNullableType()
    {
        return new GetStringRequest
        {
            Locator = Locator,
            Property = Property,
            PropertyType = PropertyType.GetInnerTypeFromNullable(),
            RawValue = RawValue,
            OwnerType = OwnerType
        };
    }

    public GetStringRequest GetRequestForElementType()
    {
        return new GetStringRequest
        {
            Locator = Locator,
            Property = Property,
            PropertyType = PropertyType.GetElementType(),
            RawValue = RawValue,
            OwnerType = OwnerType
        };
    }

    public T Get<T>()
    {
        return Locator.GetInstance<T>();
    }

    public bool Equals(GetStringRequest other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.OwnerType, OwnerType) && Equals(other.Property, Property) &&
        Equals(other.RawValue, RawValue) && Equals(other.Format, Format);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(GetStringRequest)) return false;
        return Equals((GetStringRequest)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var result = OwnerType != null ? OwnerType.GetHashCode() : 0;
            result = result * 397 ^ (Property != null ? Property.GetHashCode() : 0);
            result = result * 397 ^ (RawValue != null ? RawValue.GetHashCode() : 0);
            result = result * 397 ^ (Format != null ? Format.GetHashCode() : 0);
            return result;
        }
    }
}
