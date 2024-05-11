namespace SunamoFubuCore;

public interface IConversionRequest
{
    string Text { get; }
    T Get<T>();

    IConversionRequest AnotherRequest(string text);
}
