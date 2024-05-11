
namespace SunamoFubuCore;

[Description("Converts text by ConfigurationManager.ConnectionStrings[text] ")]
public class ResolveConnectionStringFamily : StatelessConverterBinding
{
    public override bool Matches(PropertyInfo property)
    {
        return property.HasAttribute<ConnectionStringAttribute>();
    }

    //public static Func<string, ConnectionStringSettings> GetConnectionStringSettings = key => ConfigurationManager.ConnectionStrings[key];
    private static object GetConnectionStringSettings(string name)
    {
        throw new NotImplementedException();
    }
    private static string getConnectionString(string name)
    {
        //var connectionStringSettings = GetConnectionStringSettings(name);
        //return connectionStringSettings != null
        //    ? connectionStringSettings.ConnectionString
        //    : name;

        return null;
    }



    public override object Convert(IPropertyContext context)
    {
        var stringValue = context.RawValueFromRequest.RawValue as string;

        return stringValue.IsNotEmpty()
                   ? getConnectionString(stringValue)
                   : stringValue;
    }
}
