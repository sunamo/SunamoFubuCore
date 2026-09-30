namespace SunamoFubuCore;

public class CompositeFilter<T>
{
    private readonly CompositePredicate<T> _excludes = new CompositePredicate<T>();
    private readonly CompositePredicate<T> _includes = new CompositePredicate<T>();

    public CompositePredicate<T> Includes
    {
        get => _includes;
        set { }
    }

    public CompositePredicate<T> Excludes
    {
        get => _excludes;
        set { }
    }

    public bool HasChanged => _includes.HasChanged || _excludes.HasChanged;

    public bool Matches(T target)
    {
        return Includes.MatchesAny(target) && Excludes.DoesNotMatchAny(target);
    }

    public void ResetChangeTracking()
    {
        _excludes.ResetChangeTracking();
        _includes.ResetChangeTracking();
    }

    public bool MatchesAll(T target)
    {
        return Includes.MatchesAll(target) && Excludes.DoesNotMatchAny(target);
    }

    public string GetDescriptionOfFirstMatchingInclude(T target)
    {
        return Includes.GetDescriptionOfMatches(target).FirstOrDefault();
    }
}
