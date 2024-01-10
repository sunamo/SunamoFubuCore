namespace SunamoFubuCore;



public class FlatFileWriter : IFlatFileWriter
{
    public FlatFileWriter(List<string> list)
    {
        List = list;
    }

    public List<string> List { get; }

    public void WriteProperty(string name, string value)
    {
        List.RemoveAll(x => x.StartsWith(name + "="));
        List.Add("{0}={1}".ToFormat(name, value));
    }

    public void WriteLine(string line)
    {
        List.Fill(line);
    }

    public void Sort()
    {
        List.Sort();
    }

    public void Describe()
    {
        List.Each(ConsoleWriter.Write);
    }

    public override string ToString()
    {
        var writer = new StringWriter();
        List.Each(x => writer.WriteLine(x));

        return writer.ToString();
    }
}
