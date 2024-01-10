namespace SunamoFubuCore.Conversion;

public class ConversionRequest : IConversionRequest
{
    private readonly Func<Type, object> _finder;

    public ConversionRequest(string text)
    : this(text, type => { throw new NotSupportedException("You have not registered a finder"); })
    {
        Text = text;
    }

    public ConversionRequest(string text, Func<Type, object> finder)
    {
        Text = text;
        _finder = finder;
    }

    public string Text { get; }

    public IConversionRequest AnotherRequest(string text)
    {
        return new ConversionRequest(text, _finder);
    }

    public T Get<T>()
    {
        return (T)_finder(typeof(T));
    }
}
