using System;

namespace Microsoft.Practices.Unity.Startable
{
    /// <summary>
    /// This extension tracks startable components and whenever possible (all dependencies resolved) starts them.
    /// </summary>
    public class StartableExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Registering += (s, e) => OnRegisteringType(e.TypeTo ?? e.TypeTo);
        }

        private void OnRegisteringType(Type type)
        {
            
        }
    }
}
