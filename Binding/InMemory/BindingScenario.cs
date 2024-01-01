namespace SunamoFubuCore.Binding.InMemory;




public class BindingScenario<T> where T : class, new()
{
    private readonly StringWriter _writer = new StringWriter();

    private BindingScenario(ScenarioDefinition definition)
    {
        var context = new BindingContext(definition.RequestData, definition.Services, definition.Logger);

        context.ForObject(definition.Model, () => definition.Actions.Each(x => x(context)));

        Model = definition.Model;
        Problems = context.Problems;

        History = definition.History;

        if (definition.History.AllReports.Count() == 1) Report = definition.History.AllReports.Single();
    }

    public InMemoryBindingHistory History { get; private set; }

    public BindingReport Report { get; private set; }

    public IList<ConvertProblem> Problems { get; private set; }

    public T Model { get; }

    public string Log => _writer.GetStringBuilder().ToString();

    public static BindingScenario<T> For(Action<ScenarioDefinition> configuration)
    {
        var definition = new ScenarioDefinition();
        configuration(definition);

        return new BindingScenario<T>(definition);
    }

    public static T Build(Action<ScenarioDefinition> configuration)
    {
        return For(configuration).Model;
    }

    #region Nested type: PropertyModelBinderStandin

    [Description("Strictly a fake for the binding scenario")]
    public class PropertyModelBinderStandin : IModelBinder
    {
        public bool Matches(Type type)
        {
            ThrowEx.NotImplementedMethod();
            return false;
        }

        public object Bind(Type type, IBindingContext context)
        {
            ThrowEx.NotImplementedMethod();
            return null;
        }

        public void BindProperties(Type type, object instance, IBindingContext context)
        {
        }
    }

    #endregion

    #region Nested type: ScenarioDefinition

    public class ScenarioDefinition
    {
        private readonly IList<Action<IBindingContext>> _actions = new List<Action<IBindingContext>>();
        private readonly KeyValues _data = new KeyValues();
        private readonly InMemoryServiceLocator _services = new InMemoryServiceLocator();
        private IServiceLocator _customServices;

        public ScenarioDefinition()
        {
            Logger = new RecordingBindingLogger(History);

            _services.Add<IObjectResolver>(new ObjectResolver(_services, Registry, Logger));
        }

        public InMemoryBindingHistory History { get; } = new InMemoryBindingHistory();

        public RecordingBindingLogger Logger { get; }

        public IRequestData RequestData => new RequestData(new FlatValueSource(_data));

        public IServiceLocator Services => _customServices ?? _services;

        public IEnumerable<Action<IBindingContext>> Actions
        {
            get
            {
                if (!_actions.Any())
                {
                    if (Model != null)
                        return new Action<IBindingContext>[]
                        {
context =>
new ObjectResolver(Services, Registry, new NulloBindingLogger()).BindProperties(
typeof(T), Model, context)
                        };

                    return new Action<IBindingContext>[]
                    {
context =>
{
var resolver = new ObjectResolver(Services, Registry, Logger);
Model = (T)resolver.BindModel(typeof(T), context).Value;
}
                    };
                }


                return _actions;
            }
        }

        public T Model { get; set; }

        public BindingRegistry Registry { get; } = new BindingRegistry();

        public void ServicesFrom(IServiceLocator services)
        {
            _customServices = services;
        }

        public void Service<TService>(TService service)
        {
            if (_customServices != null)
                throw new ArgumentOutOfRangeException("Cannot set services if using a pre-built IServiceLocator");

            _services.Add(service);
        }

        /// <summary>
        ///     Allows you to force load key/value pairs in the format:
        ///     prop1=val1
        ///     ChildProp1=val
        ///     Prop2=val
        ///     Prop3=val
        /// </summary>
        /// <param name="text"></param>
        public void Data(string text)
        {
            _data.ReadData(text);
        }

        public void Data(string name, object value)
        {
            _data[name] = value.ToString();
        }

        public void Data(Expression<Func<T, object>> property, object rawValue)
        {
            _data[property.ToAccessor().Name] = rawValue.ToString();
        }

        public void BindPropertyWith<TBinder>(Expression<Func<T, object>> property, string rawValue = null)
        where TBinder : IPropertyBinder, new()
        {
            BindPropertyWith(new TBinder(), property, rawValue);
        }

        public void BindPropertyWith(IPropertyBinder binder, Expression<Func<T, object>> property,
        string rawValue = null)
        {
            if (rawValue != null) Data(property, rawValue);
            var prop = property.ToAccessor().InnerProperty;
            _actions.Add(context =>
            {
                if (Model == null) Model = new T();

                context.ForObject(Model, () =>
    {
        Logger.Chose(typeof(T), new PropertyModelBinderStandin());
        StandardModelBinder.PopulatePropertyWithBinder(prop, context, binder);
        Logger.FinishedModel();
    });
            });
        }
    }

    #endregion
}
