using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Unity.TypeTracking;

namespace Microsoft.Practices.Unity.Startable
{
    /// <summary>
    ///     This extension tracks startable components and whenever possible (all dependencies resolved) starts them.
    /// </summary>
    public class StartableExtension : UnityContainerExtension
    {
        private readonly List<WeakReference<IStartable>> _startables = new List<WeakReference<IStartable>>();
 
        protected override void Initialize()
        {
            var typeTracking = Container.Configure<ITypeTrackingExtension>();
            if (typeTracking == null)
            {
                Container.AddNewExtension<TypeTrackingExtension>();
            }
            Context.Registering += OnRegisteringType;
            Container.RegisterInstance(new StartableDisposer(_startables));
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
        private static void VerifyStartableIsSingleton(Type type, LifetimeManager lifetimeManager)
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
            _startables.Add(new WeakReference<IStartable>(startable));
        }
    }
}