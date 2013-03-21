namespace unity.extensions.tests.TypeTracking
{
    public class ClassWithDependencyWithDependency
    {
        private readonly ClassWithDependency _service;

        public ClassWithDependencyWithDependency(ClassWithDependency service)
        {
            _service = service;
        }
    }
}