namespace SunamoFubuCore;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ExpandEnvironmentVariablesAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ConnectionStringAttribute : Attribute { }
