using System;

namespace Microsoft.Practices.Unity.Startable
{
    public class StartableRegistration : IStartableRegistration
    {
        private readonly WeakReference<IStartable> _reference;

        public StartableRegistration(WeakReference<IStartable> reference)
        {
            _reference = reference;
        }

        public void Start()
        {
            Do(s => s.Start());
        }

        public void Stop()
        {
            Do(s => s.Stop());
        }

        private void Do(Action<IStartable> action)
        {
            IStartable startable;
            if (_reference.TryGetTarget(out startable))
            {
                action(startable);
            }
        }
    }
}