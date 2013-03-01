using System.Collections.Generic;
using System.Reflection;
using Common.Logging;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity.Logging
{
    internal class CommonLoggingPolicy : IBuilderPolicy
    {
        private readonly IDictionary<PropertyInfo, LoggingAttribute> _loggers =
            new Dictionary<PropertyInfo, LoggingAttribute>();

        public int LoggersCount
        {
            get
            {
                return _loggers.Count;
            }
        }

        public void AddLogger(PropertyInfo prop, LoggingAttribute attr)
        {
            _loggers.Add(prop, attr);
        }

        public void SetLogger(object existing)
        {
            foreach (var prop in _loggers.Keys)
            {
                prop.SetValue(
                    existing,
                    LogManager.GetLogger(_loggers[prop].LoggerType));
            }
        }
    }
}
