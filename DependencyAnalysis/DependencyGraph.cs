namespace SunamoFubuCore;



public class DependencyGraph<T> where T : class
{
    private readonly DirectedGraph _cycleDetector;
    private readonly Func<T, IEnumerable<string>> _getDependencies;
    private readonly Func<T, string> _getName;
    private readonly IDictionary<string, T> _items;


    public DependencyGraph(Func<T, string> getName, Func<T, IEnumerable<string>> getDependencies)
    {
        _cycleDetector = new DirectedGraph();
        _items = new Dictionary<string, T>();
        _getName = getName;
        _getDependencies = getDependencies;
    }

    public void RegisterItem(T item)
    {
        var name = _getName(item);

        _items.SmartAdd(name, item);

        _cycleDetector.AddNode(new Node(name));
        foreach (var dep in _getDependencies(item))
            //bottle X needs bottle Y
            _cycleDetector.Connect(_getName(item), dep);
    }

    public bool HasCycles()
    {
        List<Cycle> cycles;
        return HasCycles(out cycles);
    }

    public bool HasCycles(out List<Cycle> cycles)
    {
        cycles = _cycleDetector.FindCycles().ToList();
        return cycles.Count() > 0;
    }

    public IEnumerable<string> MissingDependencies()
    {
        var registeredNames = _items.Keys.ToList();
        var neededNames = _cycleDetector.Nodes.Select(n => n.Name).ToList();
        var missing = neededNames.Except(registeredNames);
        return missing;
    }

    public bool HasMissingDependencies()
    {
        var missing = MissingDependencies();
        return missing.Count() > 0;
    }

    public IEnumerable<T> Ordered()
    {
        return GetLoadOrder().Select(convert).Where(x => x != null).ToList();
    }

    private T convert(string name)
    {
        if (!_items.ContainsKey(name)) return null;

        try
        {
            return _items[name];
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException("Couldn't find key '{0}' for type '{1}'".ToFormat(name, typeof(T)), ex);
        }
    }

    public IEnumerable<string> GetLoadOrder()
    {
        List<Cycle> cycles;
        if (HasCycles(out cycles))
        {
            var cycleDescription = cycles.Select(x => x.Name).Join(Environment.NewLine);
            throw new InvalidOperationException(
            @"This graph has dependency cycles and cannot be ordered!
The following cycles exist:
{0}".ToFormat(cycleDescription));
        }

        foreach (var node in _cycleDetector.Order()) yield return node.Name;
    }
}
