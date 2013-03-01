namespace Microsoft.Practices.Unity.Settings
{
    public interface ISettingExtension : IUnityContainerExtensionConfigurator
    {
        ISettingExtension AddSettingRepository<TRepository>() where TRepository : ISettingRepository;
        ISettingExtension AddSettingRepository<TRepository>(string name) where TRepository : ISettingRepository;
    }
}