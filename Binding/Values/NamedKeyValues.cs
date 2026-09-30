namespace SunamoFubuCore;

public class NamedKeyValues : GenericKeyValues
{
    public NamedKeyValues(NameValueCollection values) : base(key => values[key], () => values.AllKeys)
    {
    }
}
