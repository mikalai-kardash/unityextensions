using System;

namespace Microsoft.Practices.Unity.TypeTracking
{
    public static class TypeTrackingExtensionMethods
    {
        public static bool CanResolve<T>(this UnityContainer container)
        {
            return container
                .Configure<ITypeTrackingExtension>()
                .CanResolve<T>();
        }

        public static bool CanResolve<T>(this UnityContainer container, string name)
        {
            return container
                .Configure<ITypeTrackingExtension>()
                .CanResolve<T>(name);
        }

        public static void WhenCanBeResolved<T>(this UnityContainer container, string name, Action<T, string> action)
        {
            container
                .Configure<ITypeTrackingExtension>()
                .WhenCanBeResolved<T>(name, action);
        }

        public static void WhenCanBeResolved<T>(this UnityContainer container, Action<T, string> action)
        {
            container
                .Configure<ITypeTrackingExtension>()
                .WhenCanBeResolved<T>(action);
        }
    }
}
