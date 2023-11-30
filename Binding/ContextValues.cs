namespace FubuCore.Binding
{
    public class ContextValues : IContextValues
    {
        private readonly IObjectConverter _converter;
        private readonly IBindingLogger _logger;
        private readonly List<Func<string, string>> _namingStrategies;
        private readonly IRequestData _rawData;

        public ContextValues(IObjectConverter converter, List<Func<string, string>> namingStrategies,
            IRequestData rawData, IBindingLogger logger)
        {
            _converter = converter;
            _namingStrategies = namingStrategies;
            _rawData = rawData;
            _logger = logger;
        }

        public T ValueAs<T>(string name)
        {
            var bindingValue = RawValue(name);
            if (bindingValue == null || bindingValue.RawValue == null) return default;

            return _converter.FromString<T>(bindingValue.RawValue.ToString());
        }

        public object ValueAs(Type type, string name)
        {
            var bindingValue = RawValue(name);
            if (bindingValue == null || bindingValue.RawValue == null) return null;

            return _converter.FromString(bindingValue.RawValue.ToString(), type);
        }

        public bool ValueAs<T>(string name, Action<T> continuation)
        {
            return RawValue(name, value =>
            {
                if (value.RawValue != null)
                {
                    var convertedValue = _converter.FromString<T>(value.RawValue.ToString());
                    continuation(convertedValue);
                }
            });
        }

        public bool ValueAs(Type type, string name, Action<object> continuation)
        {
            return RawValue(name, value =>
            {
                if (value.RawValue != null)
                {
                    var convertedValue = _converter.FromString(value.RawValue.ToString(), type);
                    continuation(convertedValue);
                }
            });
        }

        public BindingValue RawValue(string name)
        {
            BindingValue value = null;
            _namingStrategies.Any(naming =>
            {
                var n = naming(name);
                return _rawData.Value(n, x => value = x);
            });

            if (value != null) _logger.UsedValue(value);

            return value;
        }

        public bool RawValue(string name, Action<BindingValue> continuation)
        {
            return _namingStrategies.Any(naming =>
            {
                var n = naming(name);
                return _rawData.Value(n, value =>
                {
                    _logger.UsedValue(value);
                    continuation(value);
                });
            });
        }
    }
}
