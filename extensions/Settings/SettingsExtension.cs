using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Microsoft.Practices.Unity.Settings
{
    /// <summary>
    ///     Adds ability of assigning setting with appropriate values at the instances of newly created objects.
    /// </summary>
    public class SettingsExtension : UnityContainerExtension, ISettingExtension
    {
        /// <summary>
        ///     Adds settings repository to the container's configuration.
        /// </summary>
        /// <typeparam name="TRepository">
        ///     Instance of <see cref="ISettingRepository" />.
        /// </typeparam>
        /// <returns></returns>
        /// <remarks>Repository is registered as singleton.</remarks>
        public ISettingExtension AddSettingRepository<TRepository>() where TRepository : ISettingRepository
        {
            Container.RegisterType<ISettingRepository, TRepository>(
                typeof(TRepository).Name,
                new ContainerControlledLifetimeManager());
            return this;
        }

        public ISettingExtension AddSettingRepository<TRepository>(string name) where TRepository : ISettingRepository
        {
            Container.RegisterType<ISettingRepository, TRepository>(
                name,
                new ContainerControlledLifetimeManager());
            return this;
        }

        protected override void Initialize()
        {
            Context.Registering += (s, e) => OnRegisteringType(e.TypeTo ?? e.TypeFrom);
            Context.Strategies.AddNew<TypeRegistrationSettingStrategy>(UnityBuildStage.Initialization);
            AddSettingRepository<ConfigurationSettingRepository>();
        }

        private void OnRegisteringType(Type type)
        {
            var policy = Context.Policies.Get<TypeSettingPolicy>(type);
            if (policy != null)
            {
                return;
            }
            policy = new TypeSettingPolicy();
            foreach (var prop in type.GetProperties())
            {
                var attr = prop.GetCustomAttributes(typeof(SettingAttribute), true);
                if (attr.Length == 0)
                {
                    continue;
                }
                var setting = (SettingAttribute)attr[0];
                policy.AddSetting(prop, setting);
            }
            Context.Policies.Set(policy, type);
        }
    }
}
