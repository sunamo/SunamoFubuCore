namespace SunamoFubuCore;

public interface IServiceLocator
{
    T GetInstance<T>();
    object GetInstance(Type type);
    T GetInstance<T>(string name);
}

public class InMemoryServiceLocator : IServiceLocator
{
    private readonly Cache<string, object> _namedServices = new Cache<string, object>();
    private readonly Cache<Type, object> _services = new Cache<Type, object>();

    public InMemoryServiceLocator()
    {
        Add<IObjectConverter>(new ObjectConverter(this, new ConverterLibrary()));
    }

    public T GetInstance<T>()
    {
        return (T)_services[typeof(T)];
    }

    public T GetInstance<T>(string name)
    {
        return (T)_namedServices[name];
    }

    public object GetInstance(Type type)
    {
        return _services[type];
    }

    public void Add<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public void Add<T>(T service, string name)
    {
        _namedServices[name] = service;
    }
}
