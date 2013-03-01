namespace Microsoft.Practices.Unity.Settings
{
    public interface ISettingRepository
    {
        string GetSetting(string name);
    }
}