using System;
using System.Collections.Generic;
using System.Linq;

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
            bool canResolveCtorDependencies = false;
            var injectionCtor = type.GetInjectionConstructor();
            if (injectionCtor != null)
            {
                canResolveCtorDependencies =
                    injectionCtor.GetParameters().All(p => CanResolve(p.ParameterType, string.Empty));
            }
            else if (null != type.GetConstructor(Type.EmptyTypes))
            {
                canResolveCtorDependencies = true;
            }
            else if (type.GetConstructors().Length == 1)
            {
                var ctor = type.GetConstructors()[0];
                canResolveCtorDependencies = ctor.GetParameters().All(p => CanResolve(p.ParameterType, string.Empty));
            }

            bool canResolveMethodDependencies = true;
            foreach (var method in type.GetInjectionMethods())
            {
                var canResolveDependencies = method.GetParameters().All(p => CanResolve(p.ParameterType, string.Empty));
                if (!canResolveDependencies)
                {
                    canResolveMethodDependencies = false;
                    break;
                }
            }
            return canResolveCtorDependencies && canResolveMethodDependencies;
        }

        public ITypeTrackingExtension WhenCanBeResolved<T>(string name, Action<T, string> onAvailableForResolution)
        {
            return this;
        }

        public ITypeTrackingExtension WhenCanBeResolved<T>(Action<T, string> onAvailableForResolution)
        {
            return this;
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
