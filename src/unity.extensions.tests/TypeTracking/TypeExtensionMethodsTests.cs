using System.Linq;
using Microsoft.Practices.Unity.TypeTracking;
using NUnit.Framework;

namespace unity.extensions.tests.TypeTracking
{
    [TestFixture]
    public class TypeExtensionMethodsTests
    {
        [Test]
        public void GetInjectionConstructor_returns_ConstructorInfo_instance()
        {
            var type = typeof(ClassWithTwoConstructors);
            Assert.That(type.GetInjectionConstructor(), Is.Not.Null);
        }

        [Test]
        public void GetInjectionConstructor_returns_null_for_class_without_injection_constructors()
        {
            var type = typeof(SimpleClass);
            Assert.That(type.GetInjectionConstructor(), Is.Null);
        }

        [Test]
        public void GetInjectionMethods_returns_no_methods_for_ClassWithoutInjectionMethods()
        {
            var type = typeof(ClassWithoutInjectionMethods);
            Assert.That(type.GetInjectionMethods(), Is.Empty);
        }

        [Test]
        public void GetInjectionMethods_returns_two_methods_for_ClassWithInjectionMethods()
        {
            var type = typeof(ClassWithInjectionMethods);
            Assert.That(type.GetInjectionMethods().Count(), Is.EqualTo(2));
        }
    }
}
