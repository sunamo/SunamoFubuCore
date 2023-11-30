namespace FubuCore.Binding.Logging
{
    public class ElementBinding : BindingReport
    {
        public ElementBinding(int index, Type elementType, IModelBinder binder) : base(elementType, binder)
        {
            Index = index;
        }

        public int Index { get; }

        public override void AcceptVisitor(IBindingReportVisitor visitor)
        {
            visitor.Element(this);

            OrderedProperties().ToList().Each(prop => prop.AcceptVisitor(visitor));

            visitor.EndElement();
        }
    }
}
