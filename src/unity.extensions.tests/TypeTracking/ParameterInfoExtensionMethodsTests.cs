using System;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.TypeTracking;
using NUnit.Framework;

namespace unity.extensions.tests.TypeTracking
{
    [TestFixture]
    public class ParameterInfoExtensionMethodsTests
    {
        [Test]
        public void GetDependencyInfo_returns_type_information_for_parameter_wiht_dependency()
        {
            Type type = typeof (SimpleClassWithConstructorAndDependency);
            ConstructorInfo ctor = type.GetInvokationConstructor();
            foreach (ParameterInfo parameter in ctor.GetParameters())
            {
                Assert.That(parameter.GetDependencyInfo(), Is.Not.Null);
                Assert.That(parameter.GetDependencyInfo().Name, Is.EqualTo("subscriptions"));
                Assert.That(parameter.GetDependencyInfo().Type, Is.EqualTo(typeof (IRepository)));
            }
        }

        [Test]
        public void GetDependencyInfo_returns_type_information_for_parameter_without_dependency()
        {
            Type type = typeof (SimpleClassWithConstructor);
            ConstructorInfo ctor = type.GetInvokationConstructor();
            foreach (ParameterInfo parameter in ctor.GetParameters())
            {
                Assert.That(parameter.GetDependencyInfo(), Is.Not.Null);
                Assert.That(parameter.GetDependencyInfo().Name, Is.EqualTo(string.Empty));
                Assert.That(parameter.GetDependencyInfo().Type, Is.EqualTo(typeof (SimpleDependency)));
            }
        }
    }

    public class SimpleClassWithConstructorAndDependency
    {
        private readonly IRepository _repository;

        public SimpleClassWithConstructorAndDependency([Dependency("subscriptions")] IRepository repository)
        {
            _repository = repository;
        }
    }
}