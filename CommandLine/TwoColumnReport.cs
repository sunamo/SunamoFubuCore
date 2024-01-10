namespace SunamoFubuCore.CommandLine;

public class TwoColumnReport
{
    private readonly Cache<string, string> _data = new Cache<string, string>();
    private readonly string _title;

    public TwoColumnReport(string title)
    {
        _title = title;
    }

    public ConsoleColor? SecondColumnColor { get; set; }

    public void Add(string first, string second)
    {
        _data[first] = second;
    }

    public void Add<T>(Expression<Func<T, object>> property, object target)
    {
        var accessor = property.ToAccessor();
        var rawValue = accessor.GetValue(target);
        Add(accessor.Name, rawValue == null ? " -- none --" : rawValue.ToString());
    }

    public void Write()
    {
        //this needs to take into account that the default console is only 80 char wide

        var firstLength = _data.GetAllKeys().Max(x => x.Length);

        CL.WriteLine();

        ConsoleWriter.PrintHorizontalLine(2);
        CL.WriteLine("    " + _title);
        ConsoleWriter.PrintHorizontalLine(2);

        var format = "    {0," + firstLength + "} -> ";
        if (!SecondColumnColor.HasValue) format += "{1}";

        _data.Each((left, right) =>
        {
            if (SecondColumnColor.HasValue)
            {
                CL.Write(format, left, right);
                CL.ForegroundColor = ConsoleColor.Cyan;
                CL.WriteLine(right);
                CL.ResetColor();
            }
            else
            {
                CL.WriteLine(format, left, right);
            }
        });

        ConsoleWriter.PrintHorizontalLine(2);
    }
}
