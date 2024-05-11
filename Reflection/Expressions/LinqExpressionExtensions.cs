namespace SunamoFubuCore;




public static class LinqExpressionExtensions
{
    public static Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>(this IPropertyOperation builder,
    Expression<Func<T, object>> path)
    {
        var memberExpression = path.GetMemberExpression(true);
        return builder.GetPredicateBuilder<T>(memberExpression);
    }

    public static Expression<Func<T, bool>> GetPredicate<T>(this IPropertyOperation operation,
    Expression<Func<T, object>> path, object value)
    {
        return operation.GetPredicateBuilder(path)(value);
    }

    public static MemberExpression ToMemberExpression<T>(this PropertyInfo property)
    {
        var lambdaParameter = Expression.Parameter(typeof(T), "entity");
        return Expression.MakeMemberAccess(lambdaParameter, property);
    }

    public static ParameterExpression GetParameter<T>(this MemberExpression memberExpression)
    {
        var outerMostMemberExpression = memberExpression;
        while (outerMostMemberExpression != null)
        {
            var parameterExpression = outerMostMemberExpression.Expression as ParameterExpression;
            if (parameterExpression != null && parameterExpression.Type == typeof(T))
                return parameterExpression;
            outerMostMemberExpression = outerMostMemberExpression.Expression as MemberExpression;
        }

        return Expression.Parameter(typeof(T), "unreferenced");
    }
}
