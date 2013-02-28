using Microsoft.Practices.Unity.Settings;

namespace unity.extensions.tests.Settings
{
    internal class CustomSettingAttribute : SettingAttribute
    {
        public CustomSettingAttribute()
            : base("Does not matter for this test.") {}

        public override string SettingRepository
        {
            get
            {
                return typeof(CustomProvider).Name;
            }
        }
    }
}
