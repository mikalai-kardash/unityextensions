namespace Microsoft.Practices.Unity.Startable
{
    public interface IStartableExtension : IUnityContainerExtensionConfigurator
    {
        IStartableExtension RegisterStartable<T>(string beginMethodName, string endMethodName);
    }
}