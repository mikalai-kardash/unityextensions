using Microsoft.Practices.Unity;

namespace unity.extensions.tests.TypeTracking
{
    public class ClassWithInjectionMethods
    {
        [InjectionMethod]
        public void FirstInjectionMethod() {}

        [InjectionMethod]
        public void SecondInjectionMethod() {}

        public void NotAnInjectionMethod() {}
    }
}
