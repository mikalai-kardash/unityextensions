using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.TypeTracking;
using NUnit.Framework;

namespace unity.extensions.tests.TypeTracking
{
    [TestFixture]
    public class TypeExtensionMethodsTests
    {
        [TestCase(typeof (ClassWithTwoCtors))]
        [TestCase(typeof (ClassWithTwoInjectionCtors))]
        public void GetInvokationConstructor_returns_null(Type type)
        {
            Assert.That(type.GetInvokationConstructor(), Is.Null);
        }

        [Test]
        public void GetInjectionConstructor_returns_ConstructorInfo_instance()
        {
            Type type = typeof (ClassWithTwoConstructors);
            Assert.That(type.GetInjectionConstructor(), Is.Not.Null);
        }

        [Test]
        public void GetInjectionConstructor_returns_null_for_class_without_injection_constructors()
        {
            Type type = typeof (SimpleClass);
            Assert.That(type.GetInjectionConstructor(), Is.Null);
        }

        [Test]
        public void GetInjectionMethods_returns_no_methods_for_ClassWithoutInjectionMethods()
        {
            Type type = typeof (ClassWithoutInjectionMethods);
            Assert.That(type.GetInjectionMethods(), Is.Empty);
        }

        [Test]
        public void GetInjectionMethods_returns_two_methods_for_ClassWithInjectionMethods()
        {
            Type type = typeof (ClassWithInjectionMethods);
            Assert.That(type.GetInjectionMethods().Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetInvokationConstructor_returns_InjectionConstructor()
        {
            Type type = typeof (ClassWithTwoConstructors);
            Assert.That(type.GetInvokationConstructor(), Is.Not.Null);
            Assert.That(type.GetInvokationConstructor().GetParameters().Length, Is.EqualTo(1));
        }

        [Test]
        public void GetInvokationConstructor_returns_default_ctor()
        {
            Type type = typeof (SimpleClass);
            Assert.That(type.GetInvokationConstructor(), Is.Not.Null);
            Assert.That(type.GetInvokationConstructor().GetParameters().Length, Is.EqualTo(0));
        }

        [Test]
        public void GetInvokationConstructor_returns_single_ctor_with_parameters()
        {
            Type type = typeof (ClassWithDependency);
            Assert.That(type.GetInvokationConstructor(), Is.Not.Null);
            Assert.That(type.GetInvokationConstructor().GetParameters().Length, Is.EqualTo(1));
        }

        [Test]
        public void GetPropertyDependencies_returns_two_properties()
        {
            Type type = typeof (ClassWithPropertyDependencies);
            Assert.That(type.GetPropertyDependencies(), Is.Not.Empty);
            Assert.That(type.GetPropertyDependencies().Count(), Is.EqualTo(2));
        }
    }

    public class ClassWithPropertyDependencies
    {
        [Dependency]
        public IRepository Repository { get; set; }

        [Dependency]
        public IService Service { get; set; }
    }
}