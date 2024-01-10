namespace SunamoFubuCore.Reflection.Expressions;

public abstract class BinaryComparisonPropertyOperation : IPropertyOperation
{
    private readonly ExpressionType _comparisonType;

    protected BinaryComparisonPropertyOperation(ExpressionType comparisonType)
    {
        _comparisonType = comparisonType;
    }

    #region IPropertyOperation Members

    public abstract string OperationName { get; }
    public abstract string Text { get; }

    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>(MemberExpression propertyPath)
    {
        return expected =>
        {
            Debug.WriteLine("Building expression for " + _comparisonType);

            var expectedHolder = propertyPath.Member is PropertyInfo
    ? Expression.Constant(expected, propertyPath.Member.As<PropertyInfo>().PropertyType)
    : Expression.Constant(expected);

            var comparison = Expression.MakeBinary(_comparisonType, propertyPath, expectedHolder);
            var lambdaParameter = propertyPath.GetParameter<T>();

            return Expression.Lambda<Func<T, bool>>(comparison, lambdaParameter);
        };
    }

    #endregion
}
