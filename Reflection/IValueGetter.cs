namespace SunamoFubuCore;

public interface IValueGetter
{
    string Name { get; }
    Type DeclaringType { get; }

    Type ValueType { get; }
    object GetValue(object target);

    Expression ChainExpression(Expression body);
    void SetValue(object target, object propertyValue);
}
