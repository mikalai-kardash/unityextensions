using unity.extensions.tests.TypeTracking;

namespace unity.extensions.tests.Startable
{
    public class StartableWithDependency : SimpleStartable
    {
// ReSharper disable NotAccessedField.Local
        private readonly IService _service;
// ReSharper restore NotAccessedField.Local

        public StartableWithDependency(IService service)
        {
            _service = service;
        }
    }
}