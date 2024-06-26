namespace SunamoFubuCore;

public class ValueDiagnosticReport : ValueReportBase
{
    public DiagnosticValue For(string propertyName)
    {
        return _values[propertyName];
    }

    public IEnumerable<DiagnosticValue> AllValues()
    {
        return _values.GetAll().OrderBy(x => x.Key);
    }

    protected override void store(string fullKey, object value)
    {
        _values[fullKey].Add(_source, value);
    }
}
