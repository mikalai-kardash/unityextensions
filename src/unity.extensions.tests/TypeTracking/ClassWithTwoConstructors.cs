using Microsoft.Practices.Unity;

namespace unity.extensions.tests.TypeTracking
{
    public class ClassWithTwoConstructors
    {
        private readonly IRepository _repository;

        public ClassWithTwoConstructors() {}

        [InjectionConstructor]
        public ClassWithTwoConstructors(IRepository repository)
        {
            _repository = repository;
        }
    }
}