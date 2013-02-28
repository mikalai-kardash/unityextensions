using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity.Settings
{
    internal class TypeSettingPolicy : IBuilderPolicy
    {
        private readonly IDictionary<PropertyInfo, SettingAttribute> _settings =
            new Dictionary<PropertyInfo, SettingAttribute>();

        public int SettingCount
        {
            get
            {
                return _settings.Count;
            }
        }

        public void AddSetting(PropertyInfo prop, SettingAttribute attr)
        {
            _settings.Add(prop, attr);
        }

        public void Apply(IBuilderContext context, object existing)
        {
            foreach (var prop in _settings.Keys.ToArray())
            {
                var setting = _settings[prop];
                try
                {
                    prop.SetValue(existing, setting.GetValue(context));
                }
                catch (Exception ex)
                {
                    _settings.Remove(prop);
                    UnableToSetProperty(prop, setting, ex);
                }
            }
        }

        [Conditional("DEBUG")]
        private void UnableToSetProperty(PropertyInfo prop, SettingAttribute attr, Exception ex)
        {
            Debug.WriteLine(
                "Unable to set '{0}' property on '{1}' with '{2}'.",
                prop.Name,
                prop.DeclaringType,
                attr);

            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
        }
    }
}
