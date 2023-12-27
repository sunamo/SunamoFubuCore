namespace SunamoFubuCore.Binding.Logging;



public class BindingReport
{
    public BindingReport(Type modelType, IModelBinder binder)
    {
        ModelType = modelType;
        Binder = binder;
    }

    public Type ModelType { get; }

    public IList<PropertyBindingReport> Properties { get; } = new List<PropertyBindingReport>();

    public IModelBinder Binder { get; }

    public PropertyBindingReport LastProperty => Properties.Last();

    public void WriteToConsole(bool showValues)
    {
        var writer = new BindingReportTextWriter(this, showValues);
        writer.Report.WriteToConsole();
    }

    public IEnumerable<PropertyBindingReport> OrderedProperties()
    {
        foreach (var prop in Properties
        .Where(x => x.Nested == null && !x.Elements.Any())
        .OrderBy(x => x.Property.Name))
            yield return prop;

        foreach (var prop in Properties
        .Where(x => x.Nested != null)
        .OrderBy(x => x.Property.Name))
            yield return prop;

        foreach (var prop in Properties
        .Where(x => x.Elements.Any())
        .OrderBy(x => x.Property.Name))
            yield return prop;
    }

    public virtual void AcceptVisitor(IBindingReportVisitor visitor)
    {
        visitor.Report(this);

        OrderedProperties().ToList().Each(prop => prop.AcceptVisitor(visitor));

        visitor.EndReport();
    }

    public void AddProperty(PropertyInfo property, IPropertyBinder binder)
    {
        var report = new PropertyBindingReport(property, binder);
        Properties.Add(report);
    }

    public PropertyBindingReport For(PropertyInfo property)
    {
        return Properties.LastOrDefault(x => property.PropertyMatches(x.Property));
    }

    public PropertyBindingReport For<T>(Expression<Func<T, object>> expression)
    {
        var property = expression.ToAccessor().InnerProperty;
        return For(property);
    }

    // TODO -- what if Value() is used outside the context of a property?
    public void Used(BindingValue value)
    {
        if (Properties.Any()) LastProperty.Used(value);
    }
}
