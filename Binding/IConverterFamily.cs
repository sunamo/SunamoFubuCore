namespace SunamoFubuCore.Binding;

public interface IConverterFamily
{
    bool Matches(PropertyInfo property);
    ValueConverter Build(IValueConverterRegistry registry, PropertyInfo property);
}
