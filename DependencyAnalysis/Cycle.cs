namespace SunamoFubuCore.DependencyAnalysis;

public class Cycle
{
    private readonly List<Node> _nodes;

    public Cycle()
    {
        _nodes = new List<Node>();
    }

    public string Name
    {
        get
        {
            return _nodes.Select(n => n.Name.ToString())
            .Aggregate((current, next) => string.Format("{0}->{1}", current, next));
        }
    }

    public int Count => _nodes.Count;

    public void Add(Node node)
    {
        _nodes.Add(node);
    }
}
