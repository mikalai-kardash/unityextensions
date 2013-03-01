using Microsoft.Practices.Unity.Settings;

namespace unity.extensions.tests.Settings
{
    internal class Tester
// ReSharper restore ClassNeverInstantiated.Local
    {
        [ConfigurationSetting("Key")]
// ReSharper disable UnusedAutoPropertyAccessor.Local
        public string Key { get; set; }

// ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
