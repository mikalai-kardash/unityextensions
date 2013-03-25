using System;

namespace Microsoft.Practices.Unity.Startable
{
    public interface IStartableExtension : IUnityContainerExtensionConfigurator
    {
        IStartableExtension RegisterStartable<T>(string name, string startMethodName, string stopMethodName);
        IStartableExtension RegisterStartable<T>(string startMethodName, string stopMethodName);
        IStartableExtension RegisterStartable(Type type, string startMethodName, string stopMethodName);
        IStartableExtension RegisterStartable(Type type, string name, string startMethodName, string stopMethodName);
    }
}