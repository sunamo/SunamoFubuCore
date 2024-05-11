namespace SunamoFubuCore;

public interface Line
{
    int Width { get; }
    void WriteToConsole();
    void Write(TextWriter writer);
}
