namespace FubuCore.Binding
{
    public class BindingRegistry : IModelBinderCache, IValueConverterRegistry, IPropertyBinderCache, DescribesItself
    {
        private readonly IList<IModelBinder> _binders = new List<IModelBinder>();

        private readonly List<IConverterFamily> _defaultFamilies = new List<IConverterFamily>
        {
            new ExpandEnvironmentVariablesFamily(),
            new ResolveConnectionStringFamily(),
            new BooleanFamily(),
            new NumericTypeFamily(),
            new ObjectTypeFamily()
        };

        private readonly IList<IConverterFamily> _families = new List<IConverterFamily>();
        private readonly Cache<Type, IModelBinder> _modelBinderCache = new Cache<Type, IModelBinder>();

        private readonly Cache<PropertyInfo, IPropertyBinder> _propertyBinderCache =
            new Cache<PropertyInfo, IPropertyBinder>();

        private readonly IList<IPropertyBinder> _propertyBinders = new List<IPropertyBinder>();
        private Lazy<IEnumerable<IPropertyBinder>> _defaultPropertyBinders;
        private Lazy<StandardModelBinder> _standardBinder;

        public BindingRegistry() : this(new ConverterLibrary(), Enumerable.Empty<IModelBinder>(),
            Enumerable.Empty<IPropertyBinder>(), Enumerable.Empty<IConverterFamily>())
        {
        }

        public BindingRegistry(ConverterLibrary converters, IEnumerable<IModelBinder> binders,
            IEnumerable<IPropertyBinder> propertyBinders, IEnumerable<IConverterFamily> converterFamilies)
        {
            configureModelBinders(binders);
            configurePropertyBinders(propertyBinders);

            _families.AddRange(converterFamilies);

            Converters = converters;
            _defaultFamilies.Add(new BasicConverterFamily(Converters));
        }

        public IPropertySetter PropertySetter => _standardBinder.Value;

        public ConverterLibrary Converters { get; }

        public void Describe(Description description)
        {
            description.Title = "Model Binding Graph";
            addModelBindersDescription(description);
        }

        IModelBinder IModelBinderCache.BinderFor(Type modelType)
        {
            return _modelBinderCache[modelType];
        }

        public IEnumerable<IPropertyBinder> AllPropertyBinders()
        {
            foreach (var propertyBinder in _propertyBinders) yield return propertyBinder;

            foreach (var propertyBinder in _defaultPropertyBinders.Value) yield return propertyBinder;
        }

        IPropertyBinder IPropertyBinderCache.BinderFor(PropertyInfo property)
        {
            return _propertyBinderCache[property];
        }

        public IEnumerable<IConverterFamily> AllConverterFamilies()
        {
            foreach (var family in _families) yield return family;

            foreach (var family in _defaultFamilies) yield return family;
        }

        public bool CanBeParsed(Type type)
        {
            return Converters.CanBeParsed(type);
        }

        ValueConverter IValueConverterRegistry.FindConverter(PropertyInfo property)
        {
            var family = AllConverterFamilies().FirstOrDefault(x => x.Matches(property));
            return family == null ? null : family.Build(this, property);
        }

        private void configurePropertyBinders(IEnumerable<IPropertyBinder> propertyBinders)
        {
            _propertyBinders.AddRange(propertyBinders);
            _propertyBinderCache.OnMissing = prop => AllPropertyBinders().FirstOrDefault(x => x.Matches(prop));

            _defaultPropertyBinders = new Lazy<IEnumerable<IPropertyBinder>>(buildDefaultPropertyBinders);
        }

        private void configureModelBinders(IEnumerable<IModelBinder> binders)
        {
            _modelBinderCache.OnMissing = type => AllModelBinders().FirstOrDefault(x => x.Matches(type));
            _standardBinder = new Lazy<StandardModelBinder>(buildStandardModelBinder);

            // DO NOT put the standard model binder at top
            _binders.AddRange(binders.Where(x => !(x is StandardModelBinder)));
        }

        private IEnumerable<IPropertyBinder> buildDefaultPropertyBinders()
        {
            var conversionPropertyBinder = new ConversionPropertyBinder(this);


            yield return new AttributePropertyBinder();
            yield return new ArrayPropertyBinder(conversionPropertyBinder);
            yield return new CollectionPropertyBinder(conversionPropertyBinder);

            yield return conversionPropertyBinder;
            yield return new NestedObjectPropertyBinder();
        }

        private StandardModelBinder buildStandardModelBinder()
        {
            return new StandardModelBinder(this, new TypeDescriptorCache());
        }

        public void Add(IModelBinder binder)
        {
            _binders.Add(binder);
        }

        public void Add(IPropertyBinder binder)
        {
            _propertyBinders.Add(binder);
        }

        public void Add(IConverterFamily family)
        {
            _families.Add(family);
        }

        public IEnumerable<IModelBinder> AllModelBinders()
        {
            foreach (var binder in _binders) yield return binder;

            yield return _standardBinder.Value;
        }

        private void addModelBindersDescription(Description description)
        {
            var list = description.AddList("ModelBinders", AllModelBinders());
            list.Label = "Model Binders (IModelBinder)";
            list.IsOrderDependent = true;
        }
    }
}
