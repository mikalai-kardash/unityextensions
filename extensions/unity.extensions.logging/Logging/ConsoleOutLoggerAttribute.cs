using System;
using Common.Logging.Simple;

namespace Microsoft.Practices.Unity.Logging
{
    public class ConsoleOutLoggerAttribute : LoggingAttribute
    {
        public override Type LoggerType
        {
            get
            {
                return typeof(ConsoleOutLogger);
            }
        }
    }
}
