using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public IStartableExtension RegisterStartable<T>(string beginMethodName, string endMethodName)
        {
            Type type = typeof (T);
            MethodInfo beginMethod = type.GetMethod(beginMethodName);
            MethodInfo endMethod = type.GetMethod(endMethodName);
            var registration = new OtherStartableRegistration(Container, type, string.Empty, beginMethod, endMethod);
            StartRegistration(registration, type);
            _registrations.Add(registration);
            return this;
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