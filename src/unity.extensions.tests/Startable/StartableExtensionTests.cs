using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Startable;
using Microsoft.Practices.Unity.TypeTracking;
using NUnit.Framework;
using unity.extensions.tests.TypeTracking;

namespace unity.extensions.tests.Startable
{
    [TestFixture]
    public class StartableExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<TypeTrackingExtension>();
            _container.AddNewExtension<StartableExtension>();
        }

        private UnityContainer _container;

        [Test]
        public void Should_not_start_startable_with_unregistered_dependency()
        {
            _container.RegisterType<StartableWithDependency>();
            Assert.That(StartableWithDependency.HasStarted, Is.False);
        }

        [Test]
        public void Should_start_startable_with_dependency_once_it_is_registered()
        {
            _container.RegisterType<StartableWithDependency>();
            _container.RegisterType<IService, SimpleService>();
            Assert.That(StartableWithDependency.HasStarted, Is.True);
        }

        [Test]
        public void Should_start_a_simple_startable()
        {
            _container.RegisterType<SimpleStartable>();
            Assert.That(SimpleStartable.HasStarted, Is.True);
        }
    }

    public class StartableWithDependency : SimpleStartable
    {
        private readonly IService _service;

        public StartableWithDependency(IService service)
        {
            _service = service;
        }
    }

    public class SimpleStartable : IStartable
    {
        public static bool HasStarted { get; set; }

        public static bool HasStopped { get; set; }

        public void Start()
        {
            HasStarted = true;
        }

        public void Stop()
        {
            HasStopped = true;
        }
    }
}