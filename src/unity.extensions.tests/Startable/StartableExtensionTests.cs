using System;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Startable;
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
            _container.AddNewExtension<StartableExtension>();
        }

        private UnityContainer _container;

        [Test]
        public void Should_not_start_startable_with_unregistered_dependency()
        {
            _container.RegisterType<StartableWithDependency>(new ContainerControlledLifetimeManager());
            Assert.That(SimpleStartable.HasStarted, Is.False);
        }

        [Test]
        public void Should_start_a_simple_startable()
        {
            _container.RegisterType<SimpleStartable>(new HierarchicalLifetimeManager());
            Assert.That(SimpleStartable.HasStarted, Is.True);
        }

        [Test]
        public void Should_start_startable_with_dependency_once_it_is_registered()
        {
            _container.RegisterType<StartableWithDependency>(new HierarchicalLifetimeManager());
            _container.RegisterType<IService, SimpleService>();
            Assert.That(SimpleStartable.HasStarted, Is.True);
        }

        [Test]
        public void Should_stop_startable_when_container_disposes()
        {
            using (var container = new UnityContainer())
            {
                container.AddNewExtension<StartableExtension>();
                container.RegisterType<SimpleStartable>(new ContainerControlledLifetimeManager());
            }
            Assert.That(SimpleStartable.HasStopped, Is.True);
        }

        [Test]
        [Conditional("DEBUG")]
        public void Should_throw_if_registering_startable_as_not_a_singleton()
        {
            Assert.Throws<InvalidOperationException>(() => _container.RegisterType<SimpleStartable>());
        }
    }
}