namespace SunamoFubuCore.Util;

public class DictionaryKeyValues<T> : IKeyValues<T>
{
    public DictionaryKeyValues()
    : this(new Dictionary<string, T>())
    {
    }

    public DictionaryKeyValues(IDictionary<string, T> dictionary)
    {
        Dictionary = dictionary;
    }

    public IDictionary<string, T> Dictionary { get; }

    public bool Has(string key)
    {
        return Dictionary.ContainsKey(key);
    }

    public T Get(string key)
    {
        return Dictionary[key];
    }

    public IEnumerable<string> GetKeys()
    {
        return Dictionary.Keys;
    }

    public bool ForValue(string key, Action<string, T> callback)
    {
        if (!Has(key)) return false;

        callback(key, Get(key));

        return true;
    }
}

public class DictionaryKeyValues : DictionaryKeyValues<string>, IKeyValues
{
    public DictionaryKeyValues()
    {
    }

    public DictionaryKeyValues(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }
}
