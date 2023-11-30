namespace FubuCore.Binding.Logging
{
    public class PropertyBindingReport
    {
        private readonly IList<BindingValue> _values = new List<BindingValue>();

        public PropertyBindingReport(PropertyInfo property, IPropertyBinder binder)
        {
            Property = property;
            Binder = binder;
        }

        public PropertyInfo Property { get; }

        public IPropertyBinder Binder { get; }

        public ValueConverter Converter { get; private set; }

        public IEnumerable<BindingValue> Values => _values;

        public BindingReport Nested { get; private set; }

        public IList<ElementBinding> Elements { get; } = new List<ElementBinding>();

        public void Chose(ValueConverter converter)
        {
            Converter = converter;
        }

        public void Used(BindingValue value)
        {
            _values.Add(value);
        }

        public BindingReport BindAsNestedChild(IModelBinder binder)
        {
            Nested = new BindingReport(Property.PropertyType, binder);
            return Nested;
        }

        public ElementBinding AddElement(Type elementType, IModelBinder binder)
        {
            var binding = new ElementBinding(Elements.Count, elementType, binder);
            Elements.Add(binding);

            return binding;
        }

        public void AcceptVisitor(IBindingReportVisitor visitor)
        {
            visitor.Property(this);

            if (Nested != null) Nested.AcceptVisitor(visitor);

            Elements.Each(elem => elem.AcceptVisitor(visitor));

            visitor.EndProperty();
        }
    }
}
