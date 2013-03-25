using System;
using System.Reflection;

namespace Microsoft.Practices.Unity.Startable
{
    public class OtherStartableRegistration : IStartableRegistration
    {
        private readonly MethodInfo _beginMethod;
        private readonly IUnityContainer _container;
        private readonly MethodInfo _endMethod;
        private readonly string _name;
        private readonly Type _type;

        public OtherStartableRegistration(
            IUnityContainer container,
            Type type,
            string name,
            MethodInfo beginMethod,
            MethodInfo endMethod)
        {
            _container = container;
            _type = type;
            _name = name;
            _beginMethod = beginMethod;
            _endMethod = endMethod;
        }

        public void Start()
        {
            _beginMethod.Invoke(GetObject(), new object[0]);
        }

        public void Stop()
        {
            _endMethod.Invoke(GetObject(), new object[0]);
        }

        private object GetObject()
        {
            return string.IsNullOrEmpty(_name)
                       ? _container.Resolve(_type)
                       : _container.Resolve(_type, _name);
        }
    }
}