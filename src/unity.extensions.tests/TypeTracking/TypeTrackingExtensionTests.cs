using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.TypeTracking;
using NUnit.Framework;

namespace unity.extensions.tests.TypeTracking
{
    [TestFixture]
    public class TypeTrackingExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<TypeTrackingExtension>();
        }

        private UnityContainer _container;

        [Test]
        public void CanResolve_ClassWithCtorAndMethodDependency()
        {
            _container.RegisterType<ClassWithCtorAndMethodDependency>();
            _container.RegisterType<IRepository, SimpleRepository>();
            _container.RegisterType<IService, SimpleService>();
            Assert.That(_container.CanResolve<ClassWithCtorAndMethodDependency>(), Is.True);
        }

        [Test]
        public void CanResolve_ClassWithTwoConstructors_if_registered_properly()
        {
            _container.RegisterType<IRepository, SimpleRepository>();
            Assert.That(_container.CanResolve<ClassWithTwoConstructors>(), Is.True);
        }

        [Test]
        public void CanResolve_RegiseredClassWithDependency()
        {
            _container.RegisterType<IRepository, SimpleRepository>();
            Assert.That(_container.CanResolve<ClassWithDependency>(), Is.True);
        }

        [Test]
        public void CanResolve_RegisteredSimpleType()
        {
            _container.RegisterType<IRepository, SimpleRepository>();
            Assert.That(_container.CanResolve<IRepository>(), Is.True);
        }

        [Test]
        public void CanResolve_RegisteredSimpleTypeWithName()
        {
            const string name = "SimpleRepository";
            _container.RegisterType<IRepository, SimpleRepository>(name);
            Assert.That(_container.CanResolve<IRepository>(name), Is.True);
        }

        [Test]
        public void CanResolve_Returns_false_for_interface()
        {
            Assert.That(_container.CanResolve<IRepository>(), Is.False);
        }

        [Test]
        public void CanResolve_Returns_false_if_dependency_is_not_registered()
        {
            Assert.That(_container.CanResolve<ClassWithDependency>(), Is.False);
        }

        [Test]
        public void CanResolve_SimpleClass()
        {
            Assert.That(_container.CanResolve<SimpleClass>(), Is.True);
        }

        [Test]
        public void CanResolve_SimpleClassWithDependency()
        {
            Assert.That(_container.CanResolve<SimpleClassWithConstructor>(), Is.True);
        }

        [Test]
        public void CanResolve_returns_false_for_ClassWithCtorAndMethodDependency()
        {
            _container.RegisterType<IRepository, SimpleRepository>();
            Assert.That(_container.CanResolve<ClassWithCtorAndMethodDependency>(), Is.False);
        }

        [Test]
        public void CanResolve_returns_false_for_ClassWithTwoConstructors()
        {
            Assert.That(_container.CanResolve<ClassWithTwoConstructors>(), Is.False);
        }

        [Test]
        public void Extension_should_analyze_dependencies_of_dependencies()
        {
            bool called = false;
            _container.WhenCanBeResolved<ClassWithDependencyWithDependency>(string.Empty, (t, n) => called = true);
            _container.RegisterType<ClassWithDependency>();
            Assert.That(called, Is.False);
        }

        [Test]
        public void Extension_should_notify_when_type_can_be_resolved()
        {
            bool called = false;
            _container.WhenCanBeResolved<ClassWithCtorAndMethodDependency>(string.Empty, (t, n) => called = true);

            _container.RegisterType<ClassWithCtorAndMethodDependency>();
            _container.RegisterType<IRepository, SimpleRepository>();
            _container.RegisterType<IService, SimpleService>();

            Assert.That(called, Is.True);
        }

        [Test]
        public void Extension_should_notify_when_type_can_be_resolved_if_instance_is_registered()
        {
            bool called = false;
            _container.WhenCanBeResolved<ClassWithCtorAndMethodDependency>(string.Empty, (t, n) => called = true);

            _container.RegisterType<ClassWithCtorAndMethodDependency>();
            _container.RegisterType<IRepository, SimpleRepository>();
            _container.RegisterInstance<IService>(new SimpleService());

            Assert.That(called, Is.True);
        }
    }
}