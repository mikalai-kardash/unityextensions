using System;
using Microsoft.Practices.Unity.Settings;

namespace unity.extensions.tests.Settings
{
    public class ThrowableRepository : ISettingRepository
    {
        public string GetSetting(string name)
        {
            throw new InvalidOperationException();
        }
    }
}