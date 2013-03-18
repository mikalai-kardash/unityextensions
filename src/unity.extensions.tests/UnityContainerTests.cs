using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace unity.extensions.tests
{
    [TestFixture]
    public class UnityContainerTests
    {
        private UnityContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = new UnityContainer();
        }

        [Test]
        public void Unity_throws_when_two_injection_ctors_are_defined()
        {
            _container.RegisterType<ClassWithTwoInjectionCtors>();
            _container.RegisterInstance(1);
            _container.RegisterInstance("aaa");
            Assert.Throws<ResolutionFailedException>(() => _container.Resolve<ClassWithTwoInjectionCtors>());
        }

        [Test]
        public void Unity_throws_when_two_constructors_with_parameters_are_defined()
        {
            _container.RegisterType<ClassWithTwoCtors>();
            _container.RegisterInstance(1);
            _container.RegisterInstance("aaaa");
            Assert.Throws<ResolutionFailedException>(() => _container.Resolve<ClassWithTwoCtors>());
        }
    }

    public class ClassWithTwoCtors
    {
        public ClassWithTwoCtors(int i)
        {
            
        }

        public ClassWithTwoCtors(string s)
        {
            
        }
    }


    public class ClassWithTwoInjectionCtors
    {
        [InjectionConstructor]
        public ClassWithTwoInjectionCtors(int i)
        {
        }

        [InjectionConstructor]
        public ClassWithTwoInjectionCtors(string a)
        {
        }
    }
}