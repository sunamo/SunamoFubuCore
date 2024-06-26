namespace SunamoFubuCore;



public interface IStringifier
{
    string GetString(GetStringRequest request);
    string GetString(object rawValue);
    void AddStrategy(StringifierStrategy strategy);
}

public class Stringifier : IStringifier
{
    private readonly List<PropertyOverrideStrategy> _overrides = new List<PropertyOverrideStrategy>();
    private readonly List<StringifierStrategy> _strategies = new List<StringifierStrategy>();

    public string GetString(GetStringRequest request)
    {
        if (request == null || request.RawValue == null || request.RawValue as string == string.Empty)
            return string.Empty;
        var propertyOverride = _overrides.FirstOrDefault(o => o.Matches(request.Property));

        if (propertyOverride != null) return propertyOverride.StringFunction(request);

        return findConverter(request)(request);
    }


    public string GetString(object rawValue)
    {
        if (rawValue == null || rawValue as string == string.Empty) return string.Empty;

        return GetString(new GetStringRequest(null, rawValue, null));
    }


    public void AddStrategy(StringifierStrategy strategy)
    {
        _strategies.Add(strategy);
    }

    private Func<GetStringRequest, string> findConverter(GetStringRequest request)
    {
        if (request.PropertyType.IsNullable())
        {
            if (request.RawValue == null) return r => string.Empty;

            return findConverter(request.GetRequestForNullableType());
        }

        if (request.PropertyType.IsArray)
        {
            if (request.RawValue == null) return r => string.Empty;

            return r =>
            {
                if (r.RawValue == null) return string.Empty;

                return r.RawValue.As<Array>().OfType<object>().Select(GetString).Join(", ");
            };
        }

        var strategy = _strategies.FirstOrDefault(x => x.Matches(request));
        return strategy == null ? toString : strategy.StringFunction;
    }

    private static string toString(GetStringRequest value)
    {
        return value.RawValue == null ? string.Empty : value.RawValue.ToString();
    }

    #region Nested type: PropertyOverrideStrategy

    public class PropertyOverrideStrategy
    {
        public Func<PropertyInfo, bool> Matches;
        public Func<GetStringRequest, string> StringFunction;
    }

    #endregion
}
