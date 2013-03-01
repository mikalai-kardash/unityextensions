using Microsoft.Practices.Unity.Settings;

namespace unity.extensions.tests.Settings
{
    internal class CustomProvider : ISettingRepository
    {
        public const string CustomProviderReturnValue = "AAA";

        public string GetSetting(string name)
        {
            return CustomProviderReturnValue;
        }
    }
}
