namespace unity.extensions.tests.Startable
{
    public class OtherStartable
    {
        public static bool HasStarted { get; set; }

        public static bool HasStopped { get; set; }

        public void Begin()
        {
            HasStarted = true;
        }

        public void End()
        {
            HasStopped = true;
        }
    }
}