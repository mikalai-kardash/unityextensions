using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity.Startable
{
    internal class StartableDisposer : IDisposable
    {
        private readonly List<IStartableRegistration> _startables;
        private bool _isDisposed;

        public StartableDisposer(List<IStartableRegistration> startables)
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
                IStartableRegistration[] registrations = _startables.ToArray();
                _startables.Clear();
                foreach (IStartableRegistration registration in registrations)
                {
                    registration.Stop();
                }
            }
            _isDisposed = true;
        }
    }
}