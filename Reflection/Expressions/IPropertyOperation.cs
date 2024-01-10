namespace SunamoFubuCore.Reflection.Expressions;

public interface IPropertyOperation
{
    string OperationName { get; }
    string Text { get; }
    Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>(MemberExpression propertyPath);
}
