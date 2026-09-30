namespace SunamoFubuCore;

/// <summary>
///     Uses the built in TypeDescriptor in .Net to convert objects from strings
/// </summary>
[Description("Converts from a string to a type using the built in System.ComponentModel.TypeDescriptor class")]
public class TypeDescripterConverterFamily : IObjectConverterFamily
{
    public bool Matches(Type type, ConverterLibrary converter)
    {
        try
        {
            return TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));
        }
        catch (Exception)
        {
            return false;
        }
    }

    public IConverterStrategy CreateConverter(Type type, Func<Type, IConverterStrategy> converterSource)
    {
        return new TypeDescriptorConversionStrategy(type);
    }

    #region Nested type: TypeDescriptorConversionStrategy

    public class TypeDescriptorConversionStrategy : IConverterStrategy, DescribesItself
    {
        private readonly TypeConverter _converter;

        public TypeDescriptorConversionStrategy(Type type)
        {
            _converter = TypeDescriptor.GetConverter(type);
            Type = type;
        }

        public Type Type { get; }

        public object Convert(IConversionRequest request)
        {
            return _converter.ConvertFromString(request.Text);
        }


        public void Describe(Description description)
        {
            description.Title = "TypeDescriptor";
            description.ShortDescription = "TypeDescripter conversion for " + Type.FullName;
        }
    }

    #endregion
}
