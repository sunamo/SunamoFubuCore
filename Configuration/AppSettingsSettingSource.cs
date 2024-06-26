namespace SunamoFubuCore;

public class AppSettingsSettingSource : ISettingsSource
{
    private readonly SettingCategory _category;

    public AppSettingsSettingSource(SettingCategory category)
    {
        _category = category;
    }

    public IEnumerable<SettingsData> FindSettingData()
    {
        //var data = new SettingsData(_category);

        //ConfigurationManager.AppSettings.AllKeys.Each(
        //key => { data[key] = ConfigurationManager.AppSettings[key]; });

        //yield return data;

        return new List<SettingsData>();
    }
}
