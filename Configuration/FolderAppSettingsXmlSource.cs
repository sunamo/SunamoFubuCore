namespace SunamoFubuCore;

public class FolderAppSettingsXmlSource : ISettingsSource
{
    private readonly string _folder;

    public FolderAppSettingsXmlSource(string folder)
    {
        _folder = folder;
    }

    public IEnumerable<SettingsData> FindSettingData()
    {
        return Directory.GetFiles(_folder, "*.config").Select(XmlSettingsParser.Parse);
    }
}
