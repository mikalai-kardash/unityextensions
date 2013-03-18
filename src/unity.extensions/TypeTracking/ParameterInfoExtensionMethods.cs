using System;
using System.Reflection;

namespace Microsoft.Practices.Unity.TypeTracking
{
    internal static class ParameterInfoExtensionMethods
    {
        public static RegisteredType GetDependencyInfo(this ParameterInfo parameter)
        {
            string name = string.Empty;
            Attribute attr = parameter.GetCustomAttribute(typeof (DependencyAttribute), true);
            if (attr != null)
            {
                name = ((DependencyAttribute) attr).Name ?? string.Empty;
            }
            return new RegisteredType(parameter.ParameterType, name);
        }
    }
}