namespace SunamoFubuCore;

public abstract class StatelessConverterBinding : IConverterFamily, ValueConverter
{
    public abstract bool Matches(PropertyInfo property);

    public ValueConverter Build(IValueConverterRegistry registry, PropertyInfo property)
    {
        return this;
    }

    public abstract object Convert(IPropertyContext context);
}
