namespace FubuCore.Binding
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpandEnvironmentVariablesAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ConnectionStringAttribute : Attribute
    {
    }
}
