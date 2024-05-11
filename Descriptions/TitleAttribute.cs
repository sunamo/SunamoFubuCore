namespace SunamoFubuCore;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class TitleAttribute : Attribute
{
    public TitleAttribute(string title)
    {
        Title = title;
    }

    public string Title { get; }
}
