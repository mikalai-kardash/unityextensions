using System.Configuration;

namespace Microsoft.Practices.Unity.Settings
{
    public class ConfigurationSettingRepository : ISettingRepository
    {
        public string GetSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
