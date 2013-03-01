using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Settings;
using NUnit.Framework;

namespace unity.extensions.tests.Settings
{
    [TestFixture]
    public class SettingsExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<SettingsExtension>();
        }

        private UnityContainer _container;

        [Test]
        public void AppSettings_Returns_Value()
        {
            _container.RegisterType<Tester>();
            var t = _container.Resolve<Tester>();
            Assert.That(t.Key, Is.EqualTo("Value"));
        }

        [Test]
        public void CustomSettings_set_value()
        {
            _container
                .Configure<ISettingExtension>()
                .AddSettingRepository<CustomProvider>();

            _container.RegisterType<CustomTester>();

            var t = _container.Resolve<CustomTester>();

            Assert.That(t.Key, Is.EqualTo(CustomProvider.CustomProviderReturnValue));
        }

        [Test]
        public void Do_not_throw_during_type_resolution_if_provider_throws()
        {
            _container
                .Configure<ISettingExtension>()
                .AddSettingRepository<ThrowableRepository>();

            _container.RegisterType<ThrowableTester>();

            var t = _container.Resolve<ThrowableTester>();

            Assert.That(t.Key, Is.EqualTo(null));
        }
    }
}
