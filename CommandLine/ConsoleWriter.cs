namespace SunamoFubuCore.CommandLine;

public static class ConsoleWriter
{
    public const int DefaultConsoleWidth = 120;

    static ConsoleWriter()
    {
        try
        {
            ConsoleBufferWidth = CL.BufferWidth;
        }
        catch
        {
            // CL.BufferWidth(get) will throw exceptions in certain circumstances
        }

        if (ConsoleBufferWidth < 10) // Mono will return 0 instead of throwing an exception
            ConsoleBufferWidth = DefaultConsoleWidth;

        HL = new string('-', ConsoleBufferWidth);
    }

    public static string HL { get; }

    public static int ConsoleBufferWidth { get; } = DefaultConsoleWidth;

    public static void Line()
    {
        CL.WriteLine();
    }

    public static void PrintHorizontalLine()
    {
        CL.WriteLine(HL);
    }

    public static void PrintHorizontalLine(int indent)
    {
        CL.WriteLine(new string(' ', indent) + HL.Substring(indent));
    }

    public static void Write(string stuff)
    {
        Write(ConsoleColor.White, stuff);
    }

    public static void WriteWithIndent(ConsoleColor color, int indent, string content)
    {
        CL.ForegroundColor = color;
        BreakIntoLines(indent, content)
        .Each(l => Console.WriteLine(l));
        CL.ResetColor();
    }

    public static void Write(ConsoleColor color, string content)
    {
        CL.ForegroundColor = color;
        BreakIntoLines(content)
        .Each(l => CL.WriteLine(l));
        CL.ResetColor();
    }


    public static void Write(string format, params string[] args)
    {
        var input = string.Format(format, args);
        Write(input);
    }

    private static string[] BreakIntoLines(int indent, string input)
    {
        if (string.IsNullOrEmpty(input)) return new string[0];

        var lines = new List<string>();


        while (input.Length > 0)
        {
            var width = ConsoleBufferWidth - indent;
            var chomp = input.Length > width ? width : input.Length;

            var c = new string(' ', indent) + input.Substring(0, chomp);

            lines.Add(c);
            input = input.Remove(0, chomp);
        }

        return lines.ToArray();
    }

    private static string[] BreakIntoLines(string input)
    {
        if (string.IsNullOrEmpty(input)) return new string[0];

        var lines = new List<string>();


        while (input.Length > 0)
        {
            var chomp = input.Length > ConsoleBufferWidth ? ConsoleBufferWidth : input.Length;
            var c = input.Substring(0, chomp);
            lines.Add(c);
            input = input.Remove(0, chomp);
        }

        return lines.ToArray();
    }

    public static void Write(ConsoleColor color, Action action)
    {
        CL.ForegroundColor = color;
        action();
        CL.ResetColor();
    }
}
