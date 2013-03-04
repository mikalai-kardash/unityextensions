using System;

namespace Microsoft.Practices.Unity.TypeTracking
{
    public interface ITypeTrackingExtension : IUnityContainerExtensionConfigurator
    {
        bool CanResolve<T>();
        bool CanResolve<T>(string name);
        bool CanResolve(Type type, string name);
    }
}