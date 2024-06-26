namespace SunamoFubuCore;

public class DisplayFormatter : IDisplayFormatter
{
    private readonly IServiceLocator _locator;
    private readonly IStringifier _stringifier;

    // IServiceLocator should be injected into the constructor as
    // a dependency
    public DisplayFormatter(IServiceLocator locator, IStringifier stringifier)
    {
        _locator = locator;
        _stringifier = stringifier;
    }

    public string GetDisplay(GetStringRequest request)
    {
        request.Locator = _locator;
        return _stringifier.GetString(request);
    }

    public string GetDisplay(Accessor accessor, object target)
    {
        var request = new GetStringRequest(accessor, target, _locator);
        return _stringifier.GetString(request);
    }

    public string GetDisplayForValue(Accessor accessor, object rawValue)
    {
        var request = new GetStringRequest(accessor, rawValue, _locator);
        return _stringifier.GetString(request);
    }
}
