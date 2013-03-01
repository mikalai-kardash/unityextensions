using System;

namespace Microsoft.Practices.Unity.Logging
{
    public abstract class LoggingAttribute : Attribute
    {
        public abstract Type LoggerType { get; }
    }
}
