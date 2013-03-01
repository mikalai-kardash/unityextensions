using System;
using Common.Logging.Simple;

namespace Microsoft.Practices.Unity.Logging
{
    public class TraceLoggerAttribute : LoggingAttribute
    {
        public override Type LoggerType
        {
            get
            {
                return typeof(TraceLogger);
            }
        }
    }
}
