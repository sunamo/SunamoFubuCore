namespace SunamoFubuCore;

[Description("Culture/localization/separator friendly conversion to number types")]
public class NumericTypeFamily : StatelessConverterBinding
{
    public override bool Matches(PropertyInfo property)
    {
        return property.PropertyType.IsNumeric();
    }

    public override object Convert(IPropertyContext context)
    {
        Type propertyType = context.Property.PropertyType;





        if (context.RawValueFromRequest != null)
        {
            var rawValue = context.RawValueFromRequest.RawValue;

            if (rawValue.GetType() == propertyType)
            {
                return rawValue;
            }

            var converter = TypeDescriptor.GetConverter(propertyType);

            if (rawValue.ToString().IsValidNumber())
            {
                var valueToConvert = removeNumericGroupSeparator(rawValue.ToString());
                return converter.ConvertFrom(valueToConvert);
            }

            return converter.ConvertFrom(rawValue);
        }

        return 0;
    }

    private static string removeNumericGroupSeparator(string value)
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        var numberSeparator = culture.NumberFormat.NumberGroupSeparator;
        return value.Replace(numberSeparator, "");
    }
}
