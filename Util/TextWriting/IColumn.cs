namespace SunamoFubuCore;

public interface IColumn
{
    int Width { get; }
    void WatchData(string contents);
    void Write(TextWriter writer, string text);
    void WriteToConsole(string text);
}
