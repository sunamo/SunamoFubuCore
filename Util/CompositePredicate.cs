namespace SunamoFubuCore.Util;

public class CompositePredicate<T>
{
    private readonly List<LoggablePredicate> _list = new List<LoggablePredicate>();

    private Func<T, bool> _matchesAll = x => true;
    private Func<T, bool> _matchesAny = x => true;
    private Func<T, bool> _matchesNone = x => false;

    public bool HasChanged { get; private set; }

    public void ResetChangeTracking()
    {
        HasChanged = false;
    }

    public void Add(Expression<Func<T, bool>> filter)
    {
        HasChanged = true;

        _matchesAll = x => _list.All(predicate => predicate.Matches(x));
        _matchesAny = x => _list.Any(predicate => predicate.Matches(x));
        _matchesNone = x => !MatchesAny(x);

        _list.Add(new LoggablePredicate(filter));
    }

    public static CompositePredicate<T> operator +(CompositePredicate<T> invokes, Expression<Func<T, bool>> filter)
    {
        invokes.Add(filter);
        return invokes;
    }

    public bool MatchesAll(T target)
    {
        return _matchesAll(target);
    }

    public bool MatchesAny(T target)
    {
        return _matchesAny(target);
    }

    public bool MatchesNone(T target)
    {
        return _matchesNone(target);
    }

    public IEnumerable<string> GetDescriptionOfMatches(T target)
    {
        return _list.Where(p => p.Matches(target)).Select(p => p.Description);
    }

    public bool DoesNotMatchAny(T target)
    {
        return _list.Count == 0 ? true : !MatchesAny(target);
    }

    private class LoggablePredicate
    {
        public LoggablePredicate(Expression<Func<T, bool>> expression)
        {
            Description = expression.Body.ToString();
            Matches = expression.Compile();
        }

        public string Description { get; }
        public Func<T, bool> Matches { get; }
    }
}
