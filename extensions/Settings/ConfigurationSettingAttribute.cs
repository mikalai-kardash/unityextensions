namespace Microsoft.Practices.Unity.Settings
{
    public class ConfigurationSettingAttribute : SettingAttribute
    {
        public ConfigurationSettingAttribute(string name)
            : base(name) {}

        public override string SettingRepository
        {
            get
            {
                return typeof(ConfigurationSettingRepository).Name;
            }
        }
    }
}
