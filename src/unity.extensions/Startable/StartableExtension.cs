using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity.TypeTracking;

namespace Microsoft.Practices.Unity.Startable
{
    /// <summary>
    ///     This extension tracks startable components and whenever possible (all dependencies resolved) starts them.
    /// </summary>
    public class StartableExtension : UnityContainerExtension, IStartableExtension
    {
        private readonly List<IStartableRegistration> _registrations = new List<IStartableRegistration>();

        public IStartableExtension RegisterStartable<T>(string startMethodName, string stopMethodName)
        {
            return RegisterStartable(typeof (T), string.Empty, startMethodName, stopMethodName);
        }

        public IStartableExtension RegisterStartable<T>(string name, string startMethodName, string stopMethodName)
        {
            return RegisterStartable(typeof (T), name ?? string.Empty, startMethodName, stopMethodName);
        }

        public IStartableExtension RegisterStartable(
            Type type,
            string name,
            string startMethodName,
            string stopMethodName)
        {
            MethodInfo startMethod = type.GetMethod(startMethodName);
            MethodInfo stopMethod = type.GetMethod(stopMethodName);
            VerifyContainerRegistration(type, name);
            VerifyStartMethod(type, startMethodName, startMethod);
            VerifyEndMethod(type, stopMethodName, stopMethod);
            var registration = new OtherStartableRegistration(Container, type, string.Empty, startMethod, stopMethod);
            StartRegistration(registration, type);
            _registrations.Add(registration);
            return this;
        }

        public IStartableExtension RegisterStartable(Type type, string startMethodName, string stopMethodName)
        {
            return RegisterStartable(type, string.Empty, startMethodName, stopMethodName);
        }

        [Conditional("DEBUG")]
// ReSharper disable UnusedParameter.Local
        private static void VerifyEndMethod(Type type, string endMethodName, MethodInfo endMethod)
// ReSharper restore UnusedParameter.Local
        {
            if (endMethod == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Stop method with name '{0}' is not implemented for '{1}'. Make sure there no typo or that method is implemented.",
                        endMethodName,
                        type));
            }
        }

        [Conditional("DEBUG")]
// ReSharper disable UnusedParameter.Local
        private static void VerifyStartMethod(Type type, string beginMethodName, MethodInfo beginMethod)
// ReSharper restore UnusedParameter.Local
        {
            if (beginMethod == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Start method with name '{0}' is not implemented for '{1}'. Make sure there is no typo or that method is implemented.",
                        beginMethodName, type));
            }
        }

        [Conditional("DEBUG")]
        private void VerifyContainerRegistration(Type type, string name)
        {
            Func<ContainerRegistration, bool> theOne = cr =>
                {
                    bool sameTime = cr.RegisteredType == type;
                    bool sameName;
                    if (string.IsNullOrEmpty(name))
                    {
                        sameName = true;
                    }
                    else
                    {
                        sameName = cr.Name == name;
                    }
                    return sameTime && sameName;
                };
            ContainerRegistration containerRegistration = Container.Registrations.FirstOrDefault(theOne);
            if (containerRegistration == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "The '{0}' with name '{1}' is not registered. Please register it first.",
                        type,
                        name));
            }
            if (!(containerRegistration.LifetimeManager is ContainerControlledLifetimeManager))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "The '{0}' type with name '{1}' is not registered as singleton. Make sure it is registered with ContainerControlledLifetimeManager or HierarchicalLifetimeManager.",
                        type,
                        name));
            }
        }

        private void StartRegistration(IStartableRegistration registration, Type type)
        {
            var typeTracking = Container.Configure<ITypeTrackingExtension>();
            if (typeTracking.CanResolve(type, string.Empty))
            {
                registration.Start();
            }
            else
            {
                typeTracking.WhenCanBeResolved(type, (t, n) => registration.Start());
            }
        }

        protected override void Initialize()
        {
            var typeTracking = Container.Configure<ITypeTrackingExtension>();
            if (typeTracking == null)
            {
                Container.AddNewExtension<TypeTrackingExtension>();
            }
            Context.Registering += OnRegisteringType;
            Container.RegisterInstance(new StartableDisposer(_registrations));
        }

        private void OnRegisteringType(object sender, RegisterEventArgs e)
        {
            Type type = e.TypeTo ?? e.TypeFrom;
            if (typeof (IStartable).IsAssignableFrom(type))
            {
                VerifyStartableIsSingleton(type, e.LifetimeManager);

                Type iface = e.TypeFrom ?? e.TypeTo;
                var typeTracking = Container.Configure<ITypeTrackingExtension>();
                if (typeTracking.CanResolve(iface, e.Name))
                {
                    Start(iface, e.Name);
                }
                else
                {
                    typeTracking.WhenCanBeResolved(iface, e.Name ?? string.Empty, (t, n) => Start(iface, e.Name));
                }
            }
        }

        [Conditional("DEBUG")]
// ReSharper disable UnusedParameter.Local
        private static void VerifyStartableIsSingleton(Type type, LifetimeManager lifetimeManager)
// ReSharper restore UnusedParameter.Local
        {
            if (!(lifetimeManager is ContainerControlledLifetimeManager))
            {
                throw new InvalidOperationException(
                    string.Format("All startables should should singletons. The '{0}' is not.", type));
            }
        }

        private void Start(Type type, string name)
        {
            var startable = (IStartable) Container.Resolve(type, name);
            startable.Start();
            var reference = new WeakReference<IStartable>(startable);
            _registrations.Add(new StartableRegistration(reference));
        }
    }
}