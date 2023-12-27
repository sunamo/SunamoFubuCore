namespace SunamoFubuCore.Binding;

public static class EnumerateFlatRequestData
{
    public static IEnumerable<IRequestData> For(IRequestData data, string prefix)
    {
        var indexer = new Indexer(prefix);

        while (data.HasChildRequest(indexer.Prefix))
        {
            yield return data.GetChildRequest(indexer.Prefix);
            indexer.Increment();
        }
    }

    public class Indexer
    {
        private readonly string _name;
        private int _index;

        public Indexer(string name)
        {
            _name = name;
            setPrefix();
        }

        public string Prefix { get; private set; }

        private void setPrefix()
        {
            Prefix = "{0}[{1}]".ToFormat(_name, _index);
        }

        public void Increment()
        {
            _index++;
            setPrefix();
        }
    }
}
