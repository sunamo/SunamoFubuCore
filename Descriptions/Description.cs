namespace
#if SunamoFubuCsProjFile
SunamoFubuCsProjFile
#else
SunamoFubuCore
#endif
;
public class Description
{
    public Description()
    {
        BulletLists = new List<BulletList>();
    }
    public Type TargetType { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public Cache<string, string> Properties { get; } = new Cache<string, string>();
    public Cache<string, Description> Children { get; } = new Cache<string, Description>();
    public IList<BulletList> BulletLists { get; }
    public bool HasExplicitShortDescription()
    {
        if (ShortDescription.IsEmpty()) return false;
        if (TargetType == null) return true;
        return ShortDescription != TargetType.FullName;
    }
    public bool HasMoreThanTitle()
    {
        return HasExplicitShortDescription() || BulletLists.Any() || Children.Any() || Properties.Any();
    }
    public BulletList AddList(string name, IEnumerable objects)
    {
        var list = new BulletList
        {
            Name = name
        };
        BulletLists.Add(list);
        objects.Each(x =>
        {
            var desc = For(x);
            list.Children.Add(desc);
        });
        return list;
    }
    public static Description For(object target)
    {
        var type = target.GetType();
        var description = new Description
        {
            TargetType = target.GetType(),
            Title = type.Name,
            ShortDescription = target.ToString()
        };
        type.ForAttribute<DescriptionAttribute>(x => description.ShortDescription = x.Description);
        type.ForAttribute<TitleAttribute>(x => description.Title = x.Title);
        (target as DescribesItself).CallIfNotNull(x => x.Describe(description));
        return description;
    }
    public static bool HasExplicitDescription(Type type)
    {
        return type.CanBeCastTo<DescribesItself>() || type.HasAttribute<DescriptionAttribute>() ||
        type.HasAttribute<TitleAttribute>();
    }
    public override string ToString()
    {
        return string.Format("{0}: {1}", Title, ShortDescription);
    }
    public void AcceptVisitor(IDescriptionVisitor visitor)
    {
        visitor.Start(this);
        BulletLists.Each(x => x.AcceptVisitor(visitor));
        visitor.End();
    }
    public bool IsMultiLevel()
    {
        return BulletLists.Any() || Children.Any(x => x.IsMultiLevel());
    }
    /// <summary>
    ///     Shortcut for doing Child[name] = Description.For(child)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="child"></param>
    public void AddChild(string name, object child)
    {
        Children[name] = For(child);
    }
}