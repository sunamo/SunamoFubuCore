namespace SunamoFubuCore.DependencyAnalysis;

/// <summary>
///     http://en.wikipedia.org/wiki/Directed_graph
/// </summary>
public class DirectedGraph
{
    private readonly HashSet<Edge> _edges;
    private readonly HashSet<Node> _nodes;

    public DirectedGraph()
    {
        _edges = new HashSet<Edge>();
        _nodes = new HashSet<Node>();
    }

    public IEnumerable<Node> Nodes => _nodes;

    public void Connect(string source, string target)
    {
        Connect(new Node(source), new Node(target));
    }

    public void Connect(Node source, Node target)
    {
        if (source.Equals(target))
            return; //self reference is ignored

        AddNode(source);
        AddNode(target);
        var edge = new Edge(source, target);

        if (!_edges.Contains(edge)) _edges.Add(edge);
    }

    private void Remove(Node node)
    {
        _nodes.Remove(node);
        _edges.RemoveWhere(e => e.Target.Equals(node));
    }

    public DirectedGraph Clone()
    {
        var result = new DirectedGraph();

        foreach (var node in _nodes)
        {
            var newNode = new Node(node.Name);
            result.AddNode(newNode);
        }

        foreach (var edge in _edges) result.Connect(edge.Source.Name, edge.Target.Name);

        return result;
    }

    public IEnumerable<Node> GetEntryNodes()
    {
        var result = new List<Node>();
        foreach (var node in _nodes)
            if (!_edges.Any(edge => edge.Source.Equals(node)))
                //you have no dependencies on you
                result.Add(node);
        return result;
    }

    public IEnumerable<Cycle> FindCycles()
    {
        var algo = new Tarjan();

        foreach (var cycle in algo.FindCycles(this))
            if (cycle.Count > 1)
                yield return cycle;
    }

    private IEnumerable<Node> GetTargetsForSource(Node node)
    {
        foreach (var edge in _edges)
            if (edge.Source.Equals(node))
                yield return edge.Target;
    }

    private IEnumerable<Node> GetSourcesForTarget(Node node)
    {
        foreach (var edge in _edges)
            if (edge.Target.Equals(node))
                yield return edge.Source;
    }

    public void AddNode(Node node)
    {
        if (!_nodes.Contains(node)) _nodes.Add(node);
    }

    public bool IsEmpty()
    {
        return _nodes.Count == 0;
    }

    public IEnumerable<Node> Order()
    {
        var order = new TopologicalSort();

        return order.Order(this);
    }

    /// <summary>
    ///     http://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm
    ///     http://stackoverflow.com/questions/261573/best-algorithm-for-detecting-cycles-in-a-directed-graph
    ///     http://stackoverflow.com/questions/546655/finding-all-cycles-in-graph
    ///     http://algowiki.net/wiki/index.php?title=Tarjan%27s_algorithm
    /// </summary>
    private class Tarjan
    {
        private readonly List<Cycle> _cycles;
        private readonly Stack<Node> _stack;
        private int _index;

        public Tarjan()
        {
            _index = 0;
            _stack = new Stack<Node>();
            _cycles = new List<Cycle>();
        }

        private void tarjan(Node v, DirectedGraph graph)
        {
            v.SetIndex(_index);
            v.SetLowLink(_index);
            _index++;
            _stack.Push(v);
            foreach (var n in graph.GetTargetsForSource(v))
                if (n.Index == -1)
                {
                    tarjan(n, graph);
                    var lowLink = Math.Min(v.LowLink, n.LowLink);
                    v.SetLowLink(lowLink);
                }
                else if (_stack.Contains(n))
                {
                    var lowLink = Math.Min(v.LowLink, n.Index);
                    v.SetLowLink(lowLink);
                }

            if (v.LowLink == v.Index)
            {
                var cycle = new Cycle();
                Node n;
                do
                {
                    n = _stack.Pop();
                    cycle.Add(n);
                } while (!v.Equals(n));

                _cycles.Add(cycle);
            }
        }

        public IEnumerable<Cycle> FindCycles(DirectedGraph graph)
        {
            foreach (var node in graph._nodes)
                tarjan(node, graph);

            return _cycles;
        }
    }

    private class TopologicalSort
    {
        private readonly IList<Node> _order;
        private DirectedGraph _graph;

        public TopologicalSort()
        {
            _order = new List<Node>();
        }

        public IEnumerable<Node> Order(DirectedGraph graph)
        {
            _graph = graph.Clone();

            while (!_graph.IsEmpty())
            {
                var nodes = _graph.GetEntryNodes();
                foreach (var node in nodes) visit(node);
            }

            return _order;
        }

        private void visit(Node node)
        {
            _graph.Remove(node);
            AddNode(node);
        }

        private void AddNode(Node node)
        {
            if (!_order.Contains(node)) _order.Add(node);
        }
    }
}
