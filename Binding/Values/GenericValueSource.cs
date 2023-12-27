using SunamoFubuCore.Binding;

namespace SunamoFubuCore.Binding.Values;

public class GenericValueSource : IValueSource
{
    private readonly Lazy<IEnumerable<string>> _keys;
    private readonly Func<string, object> _source;

    public GenericValueSource(string name, Func<string, object> source, Func<IEnumerable<string>> keys)
    {
        Provenance = name;
        _source = source;
        _keys = new Lazy<IEnumerable<string>>(keys);
    }

    public string Provenance { get; }

    public bool Has(string key)
    {
        return _keys.Value.Contains(key);
    }

    public object Get(string key)
    {
        return _source(key);
    }

    public bool HasChild(string key)
    {
        return false;
    }

    public IValueSource GetChild(string key)
    {
        throw new NotSupportedException();
    }

    public IEnumerable<IValueSource> GetChildren(string key)
    {
        return Enumerable.Empty<IValueSource>();
    }

    public void WriteReport(IValueReport report)
    {
        _keys.Value.Each(key => report.Value(key, Get(key)));
    }

    public bool Value(string key, Action<BindingValue> callback)
    {
        if (!Has(key)) return false;

        var value = new BindingValue
        {
            RawKey = key,
            RawValue = Get(key),
            Source = Provenance
        };

        callback(value);


        return true;
    }
}
