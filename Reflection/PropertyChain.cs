namespace SunamoFubuCore;



public class PropertyChain : Accessor
{
    private readonly IValueGetter[] _chain;


    public PropertyChain(IValueGetter[] valueGetters)
    {
        _chain = new IValueGetter[valueGetters.Length - 1];
        for (var i = 0; i < _chain.Length; i++) _chain[i] = valueGetters[i];

        ValueGetters = valueGetters;
    }

    public IValueGetter[] ValueGetters { get; }


    public void SetValue(object target, object propertyValue)
    {
        target = findInnerMostTarget(target);
        if (target == null) return;

        setValueOnInnerObject(target, propertyValue);
    }

    public object GetValue(object target)
    {
        target = findInnerMostTarget(target);

        if (target == null) return null;

        return ValueGetters.Last().GetValue(target);
    }

    public Type OwnerType
    {
        get
        {
            // Check if we're an indexer here
            var last = ValueGetters.Last();
            if (last is MethodValueGetter || last is IndexerValueGetter)
            {
                var nextUp = _chain.Reverse().Skip(1).FirstOrDefault() as PropertyValueGetter;
                if (nextUp != null) return nextUp.PropertyInfo.PropertyType;
            }

            var propertyGetter = _chain.Last() as PropertyValueGetter;
            if (propertyGetter != null) return propertyGetter.PropertyInfo.PropertyType;
            return InnerProperty != null ? InnerProperty.DeclaringType : null;
        }
    }

    public string FieldName
    {
        get
        {
            var last = ValueGetters.Last();
            if (last is PropertyValueGetter) return last.Name;

            var previous = ValueGetters[ValueGetters.Count() - 2];
            return previous.Name + last.Name;
        }
    }

    public Type PropertyType => ValueGetters.Last().ValueType;

    public PropertyInfo InnerProperty
    {
        get
        {
            var last = ValueGetters.Last() as PropertyValueGetter;
            return last == null ? null : last.PropertyInfo;
        }
    }

    public Type DeclaringType => _chain[0].DeclaringType;

    public Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression)
    {
        var accessor = expression.ToAccessor();
        var allGetters = Getters().Union(accessor.Getters()).ToArray();
        return new PropertyChain(allGetters);
    }

    public string[] PropertyNames
    {
        get { return ValueGetters.Select(x => x.Name).ToArray(); }
    }


    public Expression<Func<T, object>> ToExpression<T>()
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression body = parameter;

        ValueGetters.Each(getter => { body = getter.ChainExpression(body); });

        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), typeof(object));
        return (Expression<Func<T, object>>)Expression.Lambda(delegateType, body, parameter);
    }

    public Accessor Prepend(PropertyInfo property)
    {
        var list = new List<IValueGetter>
{
new PropertyValueGetter(property)
};
        list.AddRange(ValueGetters);

        return new PropertyChain(list.ToArray());
    }

    public IEnumerable<IValueGetter> Getters()
    {
        return ValueGetters;
    }


    /// <summary>
    ///     Concatenated names of all the properties in the chain.
    ///     Case.Site.Name == "CaseSiteName"
    /// </summary>
    public string Name
    {
        get { return ValueGetters.Select(x => x.Name).Join(""); }
    }

    protected virtual void setValueOnInnerObject(object target, object propertyValue)
    {
        ValueGetters.Last().SetValue(target, propertyValue);
    }


    protected object findInnerMostTarget(object target)
    {
        foreach (var info in _chain)
        {
            target = info.GetValue(target);
            if (target == null) return null;
        }

        return target;
    }


    public override string ToString()
    {
        return _chain.First().DeclaringType.FullName + _chain.Select(x => x.Name).Join(".");
    }

    public bool Equals(PropertyChain other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return ValueGetters.SequenceEqual(other.ValueGetters);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(PropertyChain)) return false;
        return Equals((PropertyChain)obj);
    }

    public override int GetHashCode()
    {
        return _chain != null ? _chain.GetHashCode() : 0;
    }
}
