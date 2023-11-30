namespace FubuCore.Binding.Logging
{
    public class BindingReportTextWriter : IBindingReportVisitor
    {
        private readonly Stack<BindingReport> _bindingStack = new Stack<BindingReport>();
        private readonly Stack<string> _descriptions = new Stack<string>();
        private readonly bool _showValues;

        public BindingReportTextWriter(BindingReport binding, bool showValues)
        {
            _showValues = showValues;

            addDivider();
            Report.AddText("Binding report for " + binding.ModelType.FullName);
            addDivider();

            if (showValues)
            {
                Report.StartColumns(3);
                Report.AddColumnData("Property", "Handler", "Values ('[RawValue]' from '[Source]'/[RawKey])");
            }
            else
            {
                Report.StartColumns(2);
                Report.AddColumnData("Property", "Handler");
            }

            addDivider();

            binding.AcceptVisitor(this);
            addDivider();
        }

        public TextReport Report { get; } = new TextReport();

        void IBindingReportVisitor.Report(BindingReport report)
        {
            _bindingStack.Push(report);
        }

        void IBindingReportVisitor.Property(PropertyBindingReport report)
        {
            _descriptions.Push("." + report.Property.Name);
            var handler = (object)report.Converter ?? report.Binder;
            write(handler, report.Values);
        }

        void IBindingReportVisitor.Element(ElementBinding binding)
        {
            _descriptions.Push("[{0}]".ToFormat(binding.Index));
            write(binding.Binder);
        }

        void IBindingReportVisitor.EndReport()
        {
            _bindingStack.Pop();
        }

        void IBindingReportVisitor.EndProperty()
        {
            _descriptions.Pop();
        }

        void IBindingReportVisitor.EndElement()
        {
            _descriptions.Pop();
        }

        private void addDivider()
        {
            Report.AddDivider('=');
        }

        private void write(object handler, IEnumerable<BindingValue> values = null)
        {
            var description = Description.For(handler).Title;

            var propertyName = _descriptions.Reverse().Join("").Replace(".[", "[").TrimStart('.');
            if (_showValues)
            {
                var valueString = values == null
                    ? string.Empty
                    : values.Select(x => "'{0}' from '{1}'/{2}".ToFormat(x.RawValue, x.Source, x.RawKey)).Join(", ");
                Report.AddColumnData(propertyName, description, valueString);
            }
            else
            {
                Report.AddColumnData(propertyName, description);
            }
        }
    }
}
