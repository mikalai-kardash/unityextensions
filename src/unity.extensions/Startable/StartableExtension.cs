using System;
using Microsoft.Practices.Unity.TypeTracking;

namespace Microsoft.Practices.Unity.Startable
{
    /// <summary>
    ///     This extension tracks startable components and whenever possible (all dependencies resolved) starts them.
    /// </summary>
    public class StartableExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Registering += OnRegisteringType;
        }

        private void OnRegisteringType(object sender, RegisterEventArgs e)
        {
            Type type = e.TypeTo ?? e.TypeFrom;
            if (typeof (IStartable).IsAssignableFrom(type))
            {
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

        private void Start(Type type, string name)
        {
            var instance = (IStartable) Container.Resolve(type, name);
            instance.Start();
        }
    }
}