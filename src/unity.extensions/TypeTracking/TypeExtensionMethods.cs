using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.Unity.TypeTracking
{
    /// <summary>
    /// Todo: Add ability to retrieve all properties with dependencies.
    /// </summary>
    public static class TypeExtensionMethods
    {
        /// <summary>
        ///     Returns constructor marked with <see cref="InjectionConstructorAttribute" />.
        /// </summary>
        /// <param name="type">Type to examine.</param>
        /// <returns>Null if there is no injection constructor or constructor instance.</returns>
        public static ConstructorInfo GetInjectionConstructor(this Type type)
        {
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var ctor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
// ReSharper restore LoopCanBeConvertedToQuery
            {
                var attrs = ctor.GetCustomAttributes(typeof(InjectionConstructorAttribute), true);
                if (attrs.Length == 0)
                {
                    continue;
                }
                return ctor;
            }
            return null;
        }

        public static IEnumerable<MethodInfo> GetInjectionMethods(this Type type)
        {
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
// ReSharper restore LoopCanBeConvertedToQuery
            {
                var attrs = method.GetCustomAttributes(typeof(InjectionMethodAttribute), true);
                if (attrs.Length == 0)
                {
                    continue;
                }
                yield return method;
            }
        }
    }
}
