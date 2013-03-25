using Microsoft.Practices.Unity.Startable;

namespace unity.extensions.tests.Startable
{
    public class SimpleStartable : IStartable
    {
        public static bool HasStarted { get; set; }

        public static bool HasStopped { get; set; }

        public void Start()
        {
            HasStarted = true;
        }

        public void Stop()
        {
            HasStopped = true;
        }
    }
}