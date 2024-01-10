namespace SunamoFubuCore.CommandLine;

[AttributeUsage(AttributeTargets.Property)]
public class FlagAliasAttribute : Attribute
{
    public FlagAliasAttribute(string longAlias, char oneLetterAlias)
    {
        LongAlias = longAlias;
        OneLetterAlias = oneLetterAlias;
    }

    public FlagAliasAttribute(char oneLetterAlias)
    {
        OneLetterAlias = oneLetterAlias;
    }

    public FlagAliasAttribute(string longAlias)
    {
        LongAlias = longAlias;
    }

    public string LongAlias { get; }

    public char? OneLetterAlias { get; }
}
