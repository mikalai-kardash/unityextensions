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
    }
}
