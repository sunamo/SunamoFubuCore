namespace FubuCore.Binding.Values
{
    public class ValueReport : ValueReportBase
    {
        private ValueSourceReport _currentReport;

        public IList<ValueSourceReport> Reports { get; } = new List<ValueSourceReport>();

        protected override void store(string fullKey, object value)
        {
            _currentReport.Store(fullKey, value);
        }

        protected override void startSource(IValueSource source)
        {
            _currentReport = new ValueSourceReport(source.Provenance);
            Reports.Add(_currentReport);
        }
    }
}
