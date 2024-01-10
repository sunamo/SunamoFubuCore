namespace SunamoFubuCore.Binding;

public interface IValueConverterRegistry
{
    ValueConverter FindConverter(PropertyInfo property);
    IEnumerable<IConverterFamily> AllConverterFamilies();

    bool CanBeParsed(Type type);
}
