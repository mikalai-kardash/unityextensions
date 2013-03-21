using System.Diagnostics;
using Microsoft.Practices.Unity;

namespace unity.extensions.tests.TypeTracking
{
    public class TestClass
    {
        [InjectionConstructor]
        public TestClass()
        {
            Debug.WriteLine(".ctor");
        }

        public string Prop1 { get; set; }

        [InjectionMethod]
        public void Method()
        {
            Debug.WriteLine(".method");
        }
    }
}