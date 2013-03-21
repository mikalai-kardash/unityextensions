using Microsoft.Practices.Unity;

namespace unity.extensions.tests.TypeTracking
{
    public class ClassWithCtorAndMethodDependency
    {
// ReSharper disable NotAccessedField.Local
        private readonly IRepository _repository;
// ReSharper restore NotAccessedField.Local

        public ClassWithCtorAndMethodDependency(IRepository repository)
        {
            _repository = repository;
        }

        [InjectionMethod]
        public void Load(IService service)
        {
        }
    }
}