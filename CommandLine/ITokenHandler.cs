namespace SunamoFubuCore;

public interface ITokenHandler
{
    string Description { get; }
    string PropertyName { get; }
    bool Handle(object input, Queue<string> tokens);

    string ToUsageDescription();
}
