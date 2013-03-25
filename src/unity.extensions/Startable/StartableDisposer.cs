using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity.Startable
{
    internal class StartableDisposer : IDisposable
    {
        private readonly List<WeakReference<IStartable>> _startables;
        private bool _isDisposed;

        public StartableDisposer(List<WeakReference<IStartable>> startables)
        {
            _startables = startables;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }
            if (isDisposing)
            {
                WeakReference<IStartable>[] references = _startables.ToArray();
                _startables.Clear();
                foreach (var reference in references)
                {
                    IStartable startable;
                    if (reference.TryGetTarget(out startable))
                    {
                        startable.Stop();
                    }
                }
            }
            _isDisposed = true;
        }
    }
}