namespace FubuCore.Binding.Values
{
    public class ValueSourceReport
    {
        public ValueSourceReport(string name)
        {
            Name = name;
            Values = new Cache<string, IList<string>>(x => new List<string>());
        }

        public string Name { get; }

        public Cache<string, IList<string>> Values { get; }

        public void Store(string key, object value)
        {
            var stored = value == null ? "NULL" : value.ToString();
            Values[key].Add(stored);
        }
    }

    public abstract class ValueReportBase : IValueReport
    {
        private readonly Stack<string> _prefixes = new Stack<string>();

        protected readonly Cache<string, DiagnosticValue> _values =
            new Cache<string, DiagnosticValue>(key => new DiagnosticValue(key));

        private string _prefix;
        protected string _source;

        public void StartSource(IValueSource source)
        {
            _prefixes.Clear();
            _source = source.Provenance;
            _prefix = string.Empty;

            startSource(source);
        }

        public void EndSource()
        {
            // no-op;
        }

        public void Value(string key, object value)
        {
            var fullKey = _prefix.IsEmpty() ? key : _prefix + "." + key;
            store(fullKey, value);
        }

        public void StartChild(string key)
        {
            pushPrefix(key);
        }

        public void EndChild()
        {
            popPrefix();
        }

        public void StartChild(string key, int index)
        {
            pushPrefix("{0}[{1}]".ToFormat(key, index));
        }

        private void pushPrefix(string prefix)
        {
            _prefixes.Push(prefix);
            resetPrefix();
        }

        private void resetPrefix()
        {
            _prefix = _prefixes.Reverse().Join(".");
        }

        private void popPrefix()
        {
            _prefixes.Pop();
            resetPrefix();
        }

        protected virtual void startSource(IValueSource source)
        {
            // no-op;
        }

        protected abstract void store(string fullKey, object value);
    }
}
