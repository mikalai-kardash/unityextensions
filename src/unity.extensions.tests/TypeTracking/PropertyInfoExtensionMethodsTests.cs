using System;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.TypeTracking;
using NUnit.Framework;

namespace unity.extensions.tests.TypeTracking
{
    [TestFixture]
    public class PropertyInfoExtensionMethodsTests
    {
        [Test]
        public void GetDependencyInfo_returns_not_null_when_dependency_attr_is_set()
        {
            Type type = typeof (ClassWithDependencyProperty);
            foreach (PropertyInfo property in type.GetProperties())
            {
                Assert.That(property.GetDependencyInfo(), Is.Not.Null);
                Assert.That(property.GetDependencyInfo().Name, Is.EqualTo("repository"));
                Assert.That(property.GetDependencyInfo().Type, Is.EqualTo(typeof (IRepository)));
            }
        }

        [Test]
        public void GetDependencyInfo_returns_not_null_when_no_dependency_attr_is_set()
        {
            Type type = typeof (ClassWithProperty);
            foreach (PropertyInfo property in type.GetProperties())
            {
                Assert.That(property.GetDependencyInfo(), Is.Not.Null);
                Assert.That(property.GetDependencyInfo().Name, Is.EqualTo(string.Empty));
                Assert.That(property.GetDependencyInfo().Type, Is.EqualTo(typeof (IRepository)));
            }
        }
    }

    public class ClassWithDependencyProperty
    {
        [Dependency("repository")]
        public IRepository Repository { get; set; }
    }

    public class ClassWithProperty
    {
        public IRepository Repository { get; set; }
    }
}