using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.Unity.TypeTracking
{
    /// <summary>
    ///     Todo: Add ability to retrieve all properties with dependencies.
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
            ConstructorInfo injectionCtor = null;
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (ConstructorInfo ctor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
// ReSharper restore LoopCanBeConvertedToQuery
            {
                object[] attrs = ctor.GetCustomAttributes(typeof (InjectionConstructorAttribute), true);
                if (attrs.Length == 0)
                {
                    continue;
                }
                if (injectionCtor != null)
                {
                    return null;
                }
                injectionCtor = ctor;
            }
            return injectionCtor;
        }

        /// <summary>
        ///     Returns the list of methods marked with <see cref="InjectionMethodAttribute" />.
        /// </summary>
        /// <param name="type">The type containing injection methods.</param>
        /// <returns>
        ///     Enumerable of <see cref="MethodInfo" />'s.
        /// </returns>
        public static IEnumerable<MethodInfo> GetInjectionMethods(this Type type)
        {
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
// ReSharper restore LoopCanBeConvertedToQuery
            {
                object[] attrs = method.GetCustomAttributes(typeof (InjectionMethodAttribute), true);
                if (attrs.Length == 0)
                {
                    continue;
                }
                yield return method;
            }
        }

        /// <summary>
        ///     Returns the constructor <see cref="UnityContainer" /> will use to instanciate the object.
        /// </summary>
        /// <param name="type">Any type.</param>
        /// <returns>
        ///     The instance of <see cref="ConstructorInfo" /> or null.
        /// </returns>
        public static ConstructorInfo GetInvokationConstructor(this Type type)
        {
            ConstructorInfo ctor = GetInjectionConstructor(type) ?? type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                ConstructorInfo[] ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                if (ctors.Length == 1)
                {
                    ctor = ctors[0];
                }
            }
            return ctor;
        }

        /// <summary>
        ///     Gathers the list of properties marked with <see cref="DependencyAttribute" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     All properties of <paramref name="type" /> that are marked with <see cref="DependencyAttribute" />.
        /// </returns>
        public static IEnumerable<PropertyInfo> GetPropertyDependencies(this Type type)
        {
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (null != property.GetCustomAttribute(typeof (DependencyAttribute), true))
                {
                    yield return property;
                }
            }
        }
    }
}