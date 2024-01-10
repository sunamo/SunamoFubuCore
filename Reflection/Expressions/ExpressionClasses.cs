namespace SunamoFubuCore.Reflection.Expressions;

public interface IArguments
{
    T Get<T>(string propertyName);
    bool Has(string propertyName);
}

public static class ConstructorBuilder
{
    public static LambdaExpression CreateSingleStringArgumentConstructor(Type concreteType)
    {
        var constructor = concreteType.GetConstructor(new[] { typeof(string) });
        if (constructor == null)
            throw new ArgumentOutOfRangeException("concreteType", concreteType,
            "Only types with a ctor(string) can be used here");

        var argument = Expression.Parameter(typeof(string), "x");

        var ctorCall = Expression.New(constructor, argument);

        var funcType = typeof(Func<,>).MakeGenericType(typeof(string), concreteType);
        return Expression.Lambda(funcType, ctorCall, argument);
    }
}

public class ConstructorFunctionBuilder<T>
{
    public Func<IArguments, T> CreateBuilder(ConstructorInfo constructor)
    {
        var args = Expression.Parameter(typeof(IArguments), "x");


        var arguments =
        constructor.GetParameters().Select(
        param => ToParameterValueGetter(args, param.ParameterType, param.Name));

        var ctorCall = Expression.New(constructor, arguments);

        var lambda = Expression.Lambda(typeof(Func<IArguments, T>), ctorCall, args);
        return (Func<IArguments, T>)lambda.Compile();
    }

    public static Expression ToParameterValueGetter(ParameterExpression args, Type type, string argName)
    {
        var method = typeof(IArguments).GetMethod("Get").MakeGenericMethod(type);
        return Expression.Call(args, method, Expression.Constant(argName));
    }
}
