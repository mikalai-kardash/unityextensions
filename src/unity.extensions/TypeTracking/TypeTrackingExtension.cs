using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity.TypeTracking
{
    public class TypeTrackingExtension : UnityContainerExtension, ITypeTrackingExtension
    {
        private readonly IDictionary<RegisteredType, RegisteredTypesList> _dependants =
            new Dictionary<RegisteredType, RegisteredTypesList>();

        private readonly IDictionary<RegisteredType, RegisteredTypesList> _dependencies =
            new Dictionary<RegisteredType, RegisteredTypesList>();

        private readonly IDictionary<RegisteredType, List<WeakReference<Action<Type, string>>>> _notifications =
            new Dictionary<RegisteredType, List<WeakReference<Action<Type, string>>>>();

        private readonly List<RegisteredType> _resolved = new List<RegisteredType>();

        public bool CanResolve<T>()
        {
            return CanResolve(typeof (T), string.Empty);
        }

        public bool CanResolve<T>(string name)
        {
            return CanResolve(typeof (T), name);
        }

        public bool CanResolve(Type type, string name)
        {
            // Check if type has no dependencies in case it is interface of abstract class
            if (type.IsInterface || type.IsAbstract)
            {
                var dependant = new RegisteredType(type, name);
                if (_dependants.ContainsKey(dependant))
                {
                    return _dependants[dependant].NoDependencies;
                }
                return false;
            }

            // Check if there is InjectionConstructor
            bool canResolveCtorDependencies = false;
            ConstructorInfo injectionCtor = type.GetInjectionConstructor();
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
                ConstructorInfo ctor = type.GetConstructors()[0];
                canResolveCtorDependencies = ctor.GetParameters().All(p => CanResolve(p.ParameterType, string.Empty));
            }

            bool canResolveMethodDependencies = true;
            foreach (MethodInfo method in type.GetInjectionMethods())
            {
                bool canResolveDependencies = method.GetParameters().All(p => CanResolve(p.ParameterType, string.Empty));
                if (!canResolveDependencies)
                {
                    canResolveMethodDependencies = false;
                    break;
                }
            }
            return canResolveCtorDependencies && canResolveMethodDependencies;
        }

        public ITypeTrackingExtension WhenCanBeResolved<T>(string name, Action<Type, string> action)
        {
            WhenCanBeResolved(typeof (T), name, action);
            return this;
        }

        public ITypeTrackingExtension WhenCanBeResolved<T>(Action<Type, string> action)
        {
            WhenCanBeResolved(typeof (T), string.Empty, action);
            return this;
        }

        public ITypeTrackingExtension WhenCanBeResolved(Type type, Action<Type, string> action)
        {
            WhenCanBeResolved(type, string.Empty, action);
            return this;
        }

        public ITypeTrackingExtension WhenCanBeResolved(Type type, string name,
                                                        Action<Type, string> action)
        {
            var key = new RegisteredType(type, name ?? string.Empty);
            var value = new WeakReference<Action<Type, string>>(action);
            if (!_notifications.ContainsKey(key))
            {
                _notifications.Add(key, new List<WeakReference<Action<Type, string>>> {value});
            }
            else
            {
                _notifications[key].Add(value);
            }
            return this;
        }

        protected override void Initialize()
        {
            Context.Registering += OnTypeRegistering;
            Context.RegisteringInstance += OnInstanceRegistering;
        }

        private void OnInstanceRegistering(object sender, RegisterInstanceEventArgs e)
        {
            UpdateDependencies(new RegisteredType(e.RegisteredType, e.Name ?? string.Empty));
            RaiseEventIfSomethingCanBeResolved();
        }

        private void OnTypeRegistering(object sender, RegisterEventArgs e)
        {
            if (e.TypeFrom != null)
            {
                if (e.TypeFrom.IsAbstract || e.TypeFrom.IsInterface)
                {
                    if (e.TypeTo == null)
                    {
                        throw new NotSupportedException(
                            "Cases with factory methods (injection factory) are not supported.");
                    }
                }
            }

            // If type was already looked at we do not need to look through it again.
            var dependant = new RegisteredType(e.TypeFrom ?? e.TypeTo, e.Name ?? string.Empty);
            if (_dependants.ContainsKey(dependant))
            {
                return;
            }

            UpdateDependencies(dependant);

            // Record all type's dependencies.
            RegisteredTypesList registeredTypesList = GetDependencies(e.TypeTo ?? e.TypeFrom);
            _dependants.Add(dependant, registeredTypesList);
            foreach (RegisteredType dependency in registeredTypesList)
            {
                if (_dependencies.ContainsKey(dependency))
                {
                    _dependencies[dependency].Add(dependant);
                }
                else
                {
                    _dependencies.Add(dependency, new RegisteredTypesList {dependant});
                }
            }

            //Raise event for dependencies that can be resolved now
            RaiseEventIfSomethingCanBeResolved();
        }

        private void UpdateDependencies(RegisteredType dependant)
        {
// See it this type is the actual dependency for some other type.
            if (_dependencies.ContainsKey(dependant))
            {
                RegisteredType dependency = dependant;
                RegisteredTypesList dependants = _dependencies[dependency];
                foreach (RegisteredType d in dependants)
                {
                    _dependants[d].Remove(dependency);
                    if (_dependants[d].NoDependencies)
                    {
                        SetToRaiseEventCanBeResolved(d);
                    }
                }
            }
        }


        private void RaiseEventIfSomethingCanBeResolved()
        {
            foreach (RegisteredType type in _resolved)
            {
                if (_notifications.ContainsKey(type))
                {
                    foreach (var action in _notifications[type])
                    {
                        Action<Type, string> a;
                        if (action.TryGetTarget(out a))
                        {
                            a(type.Type, type.Name);
                        }
                    }
                    _notifications.Remove(type);
                }
            }
            _resolved.Clear();
        }

        private void SetToRaiseEventCanBeResolved(RegisteredType registration)
        {
            _resolved.Add(registration);
        }

        private RegisteredTypesList GetDependencies(Type type)
        {
            var dependencies = new RegisteredTypesList();
            ConstructorInfo initCtor = type.GetInvokationConstructor();
            if (initCtor != null)
            {
                foreach (ParameterInfo parameter in initCtor.GetParameters())
                {
                    RegisteredType dependency = parameter.GetDependencyInfo();
                    if (!CanResolve(dependency.Type, dependency.Name))
                    {
                        dependencies.Add(dependency);
                    }
                }
            }
            foreach (MethodInfo method in type.GetInjectionMethods())
            {
                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    RegisteredType dependency = parameter.GetDependencyInfo();
                    if (!CanResolve(dependency.Type, dependency.Name))
                    {
                        dependencies.Add(dependency);
                    }
                }
            }
            foreach (PropertyInfo property in type.GetPropertyDependencies())
            {
                RegisteredType dependency = property.GetDependencyInfo();
                if (!CanResolve(dependency.Type, dependency.Name))
                {
                    dependencies.Add(dependency);
                }
            }
            return dependencies;
        }
    }
}