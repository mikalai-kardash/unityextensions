using System;
using System.Reflection;

namespace Microsoft.Practices.Unity.TypeTracking
{
    internal static class PropertyInfoExtensionMethods
    {
        public static RegisteredType GetDependencyInfo(this PropertyInfo property)
        {
            string name = string.Empty;
            Attribute attr = property.GetCustomAttribute(typeof (DependencyAttribute), true);
            if (attr != null)
            {
                name = ((DependencyAttribute) attr).Name;
            }
            return new RegisteredType(property.PropertyType, name);
        }
    }
}