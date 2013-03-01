using Common.Logging;
using Microsoft.Practices.Unity.Logging;

namespace unity.extensions.tests.Logging
{
    public class LoggerTester
    {
        [ConsoleOutLogger]
        public ILog Log { get; set; }
    }
}