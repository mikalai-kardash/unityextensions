using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Microsoft.Practices.Unity.TypeTracking
{
    public class TypeTrackingExtension : UnityContainerExtension, ITypeTrackingExtension
    {
        private readonly IDictionary<Dependant, Dependencies> _registrations =
            new Dictionary<Dependant, Dependencies>();

        public bool CanResolve<T>()
        {
            return CanResolve(typeof(T), string.Empty);
        }

        public bool CanResolve<T>(string name)
        {
            return CanResolve(typeof(T), name);
        }

        public bool CanResolve(Type type, string name)
        {
            // Check if type has no dependencies in case it is interface of abstract class
            if (type.IsInterface || type.IsAbstract)
            {
                var dependant = new Dependant(type, name);
                if (_registrations.ContainsKey(dependant))
                {
                    return _registrations[dependant].NoDependencies;
                }
                return false;
            }

            // Check if there is InjectionConstructor
            foreach (var ctor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
            {
                var attrs = ctor.GetCustomAttributes(typeof(InjectionConstructorAttribute), true);
                if (attrs.Length == 0)
                {
                    continue;
                }
                return ctor.GetParameters().All(p => CanResolve(p.ParameterType, string.Empty));
            }

            // Check if type has default constructor.
            if (null != type.GetConstructor(Type.EmptyTypes))
            {
                return true;
            }

            // Check if type has the only constructor.
            var ctors = type.GetConstructors();
            if (ctors.Length == 1)
            {
                var ctor = ctors[0];
                return ctor.GetParameters().All(p => CanResolve(p.ParameterType, string.Empty));
            }
            return false;
        }

        protected override void Initialize()
        {
            Context.Registering += OnTypeRegistering;
        }

        private void OnTypeRegistering(object sender, RegisterEventArgs e)
        {
            if (e.TypeFrom != null)
            {
                if (e.TypeFrom.IsInterface || e.TypeFrom.IsAbstract)
                {
                    var dependant = new Dependant(e.TypeFrom, e.Name ?? string.Empty);
                    if (CanResolve(e.TypeTo, e.Name))
                    {
                        _registrations.Add(dependant, Dependencies.None);
                    }
                }
            }
        }
    }
}
