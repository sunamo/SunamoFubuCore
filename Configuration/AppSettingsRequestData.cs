namespace SunamoFubuCore;

public class AppSettingsRequestData
{
    [Obsolete("Use the value on AppSettingsProvider instead")]
    public static string GetValueFor<T>(Expression<Func<T, object>> property)
    {
        return AppSettingsProvider.GetValueFor(property);
    }
}
