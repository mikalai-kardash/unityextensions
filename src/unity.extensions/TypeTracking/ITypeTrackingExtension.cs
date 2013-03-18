using System;

namespace Microsoft.Practices.Unity.TypeTracking
{
    public interface ITypeTrackingExtension : IUnityContainerExtensionConfigurator
    {
        bool CanResolve<T>();
        bool CanResolve<T>(string name);
        bool CanResolve(Type type, string name);

        ITypeTrackingExtension WhenCanBeResolved<T>(string name, Action<Type, string> action);
        ITypeTrackingExtension WhenCanBeResolved<T>(Action<Type, string> action);
        ITypeTrackingExtension WhenCanBeResolved(Type type, Action<Type, string> action);
        ITypeTrackingExtension WhenCanBeResolved(Type type, string name, Action<Type, string> action);
    }
}