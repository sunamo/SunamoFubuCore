namespace SunamoFubuCore;



public class ColumnDefinition
{
    public ColumnDefinition(Accessor accessor)
    {
        Accessor = accessor;
    }

    public Accessor Accessor { get; }

    public string Alias { get; set; }

    public string Name
    {
        get
        {
            if (Alias.IsNotEmpty()) return Alias;
            return Accessor.Name;
        }
    }

    public override string ToString()
    {
        return "Column {0}:{1}".ToFormat(Name, Accessor.Name);
    }
}
