namespace SunamoFubuCore.Binding;

[Description("Passthrough of System.Object properties")]
public class ObjectTypeFamily : StatelessConverterBinding
{
    public override bool Matches(PropertyInfo property)
    {
        return property.PropertyType == typeof(object);
    }

    public override object Convert(IPropertyContext context)
    {
        if (context.RawValueFromRequest == null) return null;
        return context.RawValueFromRequest.RawValue;
    }
}
