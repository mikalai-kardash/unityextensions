using Microsoft.Practices.Unity.Settings;

namespace unity.extensions.tests.Settings
{
    public class ThrowableAttribute : SettingAttribute
    {
        public ThrowableAttribute()
            : base("I Will Throw! Be sure...") {}

        public override string SettingRepository
        {
            get
            {
                return typeof(ThrowableRepository).Name;
            }
        }
    }
}