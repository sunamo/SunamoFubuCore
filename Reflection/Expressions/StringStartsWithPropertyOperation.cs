namespace SunamoFubuCore;



public class StringStartsWithPropertyOperation : CaseInsensitiveStringMethodPropertyOperation
{
    private static readonly MethodInfo _method =
    ReflectionHelper.GetMethod<string>(s => s.StartsWith("", StringComparison.CurrentCulture));

    public StringStartsWithPropertyOperation()
    : base(_method)
    {
    }

    public override string Text => "starts with";
}

public class CollectionContainsPropertyOperation : IPropertyOperation
{
    private const string _operationName = "Contains";
    private const string _description = "contains";

    private readonly MethodInfo method =
    typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(
    m => m.Name.EqualsIgnoreCase("Contains")).First();

    public string OperationName => _operationName;

    public string Text => _description;

    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>(MemberExpression propertyPath)
    {
        return valuesToCheck =>
        {
            var enumerationOfObjects = (IEnumerable<object>)valuesToCheck;
            if (enumerationOfObjects == null) return c => false;

            //what's the type of the collection?
            var valuesToCheckType = valuesToCheck.GetType();
            var collectionOf = valuesToCheckType.IsAnEnumerationOf();


            //capture and close the Enumerbable.Contains method
            var closedMethod = method.MakeGenericMethod(collectionOf);

            //the list that we need to call contains on
            var list = Expression.Constant(enumerationOfObjects);


            //lambda parameter
            var param = Expression.Parameter(typeof(T));

            //this should be a property call
            var memberAccess = Expression.MakeMemberAccess(param, propertyPath.Member);


            //call 'Contains' with the desired 'value' to check on the 'list'
            var call = Expression.Call(closedMethod, list, memberAccess);


            var lambda = Expression.Lambda<Func<T, bool>>(call, param);
            //return enumerationOfObjects.Contains(((PropertyInfo) propertyPath.Member).GetValue(c, null));
            return lambda;
        };
    }
}

public class StringContainsPropertyOperation : StringContainsPropertyOperationBase
{
    public StringContainsPropertyOperation() : base("Contains", "contains", false)
    {
    }
}

public class StringDoesNotContainPropertyOperation : StringContainsPropertyOperationBase
{
    public StringDoesNotContainPropertyOperation() : base("DoesNotContain", "does not contain", true)
    {
    }
}

public abstract class StringContainsPropertyOperationBase : IPropertyOperation
{
    private static readonly MethodInfo _indexOfMethod;
    private readonly bool _negate;

    static StringContainsPropertyOperationBase()
    {
        _indexOfMethod =
        ReflectionHelper.GetMethod<string>(s => s.IndexOf("", StringComparison.InvariantCultureIgnoreCase));
    }

    protected StringContainsPropertyOperationBase(string operation, string description, bool negate)
    {
        OperationName = operation;
        Text = description;
        _negate = negate;
    }

    public string OperationName { get; }

    public string Text { get; }

    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>(MemberExpression propertyPath)
    {
        return valueToCheck =>
        {
            var valueToCheckConstant = Expression.Constant(valueToCheck);
            var indexOfCall =
    Expression.Call(Expression.Coalesce(propertyPath, Expression.Constant(string.Empty)),
    _indexOfMethod,
    valueToCheckConstant,
    Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            var operation = _negate ? ExpressionType.LessThan : ExpressionType.GreaterThanOrEqual;
            var comparison = Expression.MakeBinary(operation, indexOfCall,
    Expression.Constant(0));
            var lambdaParameter = propertyPath.GetParameter<T>();
            return Expression.Lambda<Func<T, bool>>(comparison, lambdaParameter);
        };
    }
}
