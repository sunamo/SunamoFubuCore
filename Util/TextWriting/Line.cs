namespace SunamoFubuCore.Util.TextWriting;

public interface Line
{
    int Width { get; }
    void WriteToConsole();
    void Write(TextWriter writer);
}
