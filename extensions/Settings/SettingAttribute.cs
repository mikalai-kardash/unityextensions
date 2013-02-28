using System;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity.Settings
{
    public abstract class SettingAttribute : Attribute
    {
        protected SettingAttribute(string name)
        {
            Name = name;
        }

        public abstract string SettingRepository { get; }

        public string Name { get; private set; }

        public object GetValue(IBuilderContext context)
        {
            var repository = context.NewBuildUp<ISettingRepository>(SettingRepository);
            return repository.GetSetting(Name);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", GetType().Name, Name);
        }
    }
}
