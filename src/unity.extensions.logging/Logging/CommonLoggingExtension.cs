using System;
using Common.Logging;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Microsoft.Practices.Unity.Logging
{
    public class CommonLoggingExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Registering += (s, e) => OnRegisteringType(e.TypeTo ?? e.TypeFrom);
            Context.Strategies.AddNew<CommonLoggingExtensionStrategy>(UnityBuildStage.Initialization);
        }

        private void OnRegisteringType(Type type)
        {
            var policy = Context.Policies.Get<CommonLoggingPolicy>(type);
            if (policy != null)
            {
                return;
            }
            policy = new CommonLoggingPolicy();
            foreach (var prop in type.GetProperties())
            {
                if (prop.PropertyType != typeof(ILog))
                {
                    continue;
                }
                var attrs = prop.GetCustomAttributes(typeof(LoggingAttribute), true);
                if (attrs.Length == 0)
                {
                    continue;
                }
                policy.AddLogger(prop, (LoggingAttribute)attrs[0]);
            }
            Context.Policies.Set(policy, type);
        }
    }
}
