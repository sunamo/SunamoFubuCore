namespace SunamoFubuCore;

public interface IRequestData
{
    object Value(string key);
    bool Value(string key, Action<BindingValue> callback);
    bool HasChildRequest(string key);

    IRequestData GetChildRequest(string prefixOrChild);
    IEnumerable<IRequestData> GetEnumerableRequests(string prefixOrChild);
    void AddValues(string name, IKeyValues values);
    void AddValues(IValueSource source);
    IValueSource ValuesFor(string nameOrProvenance);

    void WriteReport(IValueReport report);
}
